BEGIN TRAN T1;

DECLARE
    @fundingStreamId INT,
    @fileTypeSettingId INT,
    @fileSizeSettingId INT,
    @currentDateTime DATETIME = GETDATE(),
    @migrationUser nvarchar(128) = 'Migration'

IF NOT EXISTS (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'NMSS')
BEGIN
    -- Funding stream
    INSERT FundingStreams (FundingStreamCode,
					       FundingStreamName,
					       Active,
					       CreatedAt,
					       LastUpdatedAt,
					       LastUpdatedBy,
					       DeletedAt,
					       FundingStreamCodePubliclyKnown,
					       FundingStreamNameWithinSentence,
					       RelevantForNational,
					       RelevantForOrganisations_LoggedIn,
					       RelevantForOrganisations_Public,
					       RelevantForProviders_LoggedIn,
					       RelevantForProviders_Public,
					       HistoryIndependentOfPublications)
    VALUES ('NMSS',
		    'Non maintained special school funding',
		    1,
		    @currentDateTime,
		    @currentDateTime,
		    @migrationUser,
		    NULL,
		    0,
		    'Non maintained special school funding',
		    0,
		    0,
		    0,
		    1,
		    0,
		    1)
END

SELECT @fundingStreamId = Id FROM FundingStreams WHERE FundingStreamCode = 'NMSS'

if NOT EXISTS (SELECT Id FROM Settings WHERE SettingName = 'FundingDocumentFileType')
BEGIN
		INSERT INTO [Settings]
           ([SettingName]
           ,[SettingDescription]
           ,[ValueDataType]
           ,[ValuesAreEditable]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           ('FundingDocumentFileType',
		   'The funding document type',
		   0,
		   1,
		   @currentDateTime,
		   @currentDateTime,
		   @migrationUser)
END

if NOT EXISTS (SELECT Id FROM Settings WHERE SettingName = 'ProviderDownloadSizeInBytes')
BEGIN
		INSERT INTO [Settings]
           ([SettingName]
           ,[SettingDescription]
           ,[ValueDataType]
           ,[ValuesAreEditable]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           ('ProviderDownloadSizeInBytes',
		   'The download size (in bytes) for the provider spreadsheet.',
		   1,
		   1,
		   @currentDateTime,
		   @currentDateTime,
		   @migrationUser)
END

SELECT @fileTypeSettingId = Id FROM Settings WHERE SettingName = 'FundingDocumentFileType'
SELECT @fileSizeSettingId = Id FROM Settings WHERE SettingName = 'ProviderDownloadSizeInBytes'

-- Update file type setting
IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @fileTypeSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        UPDATE SettingValues SET
            [Value] = 'csv',
            LastUpdatedAt = @currentDateTime,
            LastUpdatedBy = @migrationUser
        WHERE FundingStreamId = @fundingStreamId
          AND SettingId = @fileTypeSettingId
    END
ELSE
    BEGIN
        INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
        VALUES (@fundingStreamId, @fileTypeSettingId, 'csv', @currentDateTime, @currentDateTime, @migrationUser)
    END


-- Update file size setting
IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @fileSizeSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        UPDATE SettingValues SET
            [Value] = '210944',
            LastUpdatedAt = @currentDateTime,
            LastUpdatedBy = @migrationUser
        WHERE FundingStreamId = @fundingStreamId
          AND SettingId = @fileSizeSettingId
    END
ELSE
    BEGIN
        INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
        VALUES (@fundingStreamId, @fileSizeSettingId, '210944', @currentDateTime, @currentDateTime, @migrationUser)
    END

COMMIT TRAN T1;