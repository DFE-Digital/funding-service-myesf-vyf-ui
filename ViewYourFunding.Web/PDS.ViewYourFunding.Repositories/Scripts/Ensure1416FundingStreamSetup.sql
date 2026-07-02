BEGIN TRAN T1;

DECLARE
	@currentDateTime					DATETIME = GETDATE(),
	@existingFundingStreamId			INT,
	@fundingStreamId					INT,
	@spreadsheetSettingId				INT,
	@staticDataSettingId				INT,
	@parentProviderSettingId			INT,
	@academySchoolAcademicYearSettingId	INT,
	@migrationUser						NVARCHAR(128) =	'Migration'

-- Remove any existing references
IF EXISTS (SELECT Id FROM FundingStreams WHERE FundingStreamCode = '1416')
BEGIN
	DELETE Publications WHERE FundingStreamId IN (SELECT Id FROM FundingStreams WHERE FundingStreamCode = '1416')
	DELETE SettingValues WHERE FundingStreamId IN (SELECT Id FROM FundingStreams WHERE FundingStreamCode = '1416')
	DELETE FundingStreams WHERE FundingStreamCode = '1416'
END

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
VALUES ('1416',
		'14 to 16 funding',
		1,
		@currentDateTime,
		@currentDateTime,
		@migrationUser,
		NULL,
		0,
		'14 to 16 funding',
		0,
		0,
		0,
		1,
		0,
		1)

SELECT @fundingStreamId = @@IDENTITY

if NOT EXISTS (SELECT Id FROM Settings WHERE SettingName = 'UpdateSpreadsheetUrl')
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
           ('UpdateSpreadsheetUrl',
		   'The url to request to trigger this funding stream''s spreadsheet(s) to be generated.',
		   0,
		   0,
		   @currentDateTime,
		   @currentDateTime,
		   @migrationUser)
END

if NOT EXISTS (SELECT Id FROM Settings WHERE SettingName = 'UseStaticData')
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
           ('UseStaticData',
		   'Determines whether to use hard coded static data as a data source.',
		   3,
		   1,
		   @currentDateTime,
		   @currentDateTime,
		   @migrationUser)
END

if NOT EXISTS (SELECT Id FROM Settings WHERE SettingName = 'ParentProviderType')
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
           ('ParentProviderType',
		   'The parent provider type, e.g. LocalAuthority.',
		   0,
		   1,
		   @currentDateTime,
		   @currentDateTime,
		   @migrationUser)
END


if NOT EXISTS (SELECT Id FROM Settings WHERE SettingName = 'AcademyAndSchoolAcademicYear')
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
           ('AcademyAndSchoolAcademicYear',
		   'The academy academic year of the allocations that should be shown, e.g. 201819.',
		   0,
		   1,
		   @currentDateTime,
		   @currentDateTime,
		   @migrationUser)
END


-- Settings
SELECT @spreadsheetSettingId = Id FROM Settings WHERE SettingName = 'UpdateSpreadsheetUrl'
SELECT @staticDataSettingId = Id FROM Settings WHERE SettingName = 'UseStaticData'
SELECT @parentProviderSettingId = Id FROM Settings WHERE SettingName = 'ParentProviderType'
SELECT @academySchoolAcademicYearSettingId = Id FROM Settings WHERE SettingName = 'AcademyAndSchoolAcademicYear'


IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @spreadsheetSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        UPDATE SettingValues SET
            [Value] = '/view-latest-funding/api/funding/GenerateFundingDocument?fundingStreamCode={0}&fundingPeriodCode={1}&cutoffDate={2}&publicationDate={3}&modelVersion={4}&waitForIndexBuild=true',
            LastUpdatedAt = @currentDateTime,
            LastUpdatedBy = @migrationUser
        WHERE FundingStreamId = @fundingStreamId
          AND SettingId = @spreadsheetSettingId
    END
ELSE
	BEGIN
		INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
		VALUES (@fundingStreamId, @spreadsheetSettingId, '/view-latest-funding/api/funding/GenerateFundingDocument?fundingStreamCode={0}&fundingPeriodCode={1}&cutoffDate={2}&publicationDate={3}&modelVersion={4}&waitForIndexBuild=true', @currentDateTime, @currentDateTime, @migrationUser)
	END


IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @staticDataSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        UPDATE SettingValues SET
            [Value] = 'true',
            LastUpdatedAt = @currentDateTime,
            LastUpdatedBy = @migrationUser
        WHERE FundingStreamId = @fundingStreamId
          AND SettingId = @staticDataSettingId
    END
ELSE
	BEGIN
		INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
		VALUES (@fundingStreamId, @staticDataSettingId, 'true', @currentDateTime, @currentDateTime, @migrationUser)
	END


IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @parentProviderSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        UPDATE SettingValues SET
            [Value] = 'LocalAuthority',
            LastUpdatedAt = @currentDateTime,
            LastUpdatedBy = @migrationUser
        WHERE FundingStreamId = @fundingStreamId
          AND SettingId = @parentProviderSettingId
    END
ELSE
	BEGIN
		INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
		VALUES (@fundingStreamId, @parentProviderSettingId, 'LocalAuthority', @currentDateTime, @currentDateTime, @migrationUser)
	END


IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @academySchoolAcademicYearSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        UPDATE SettingValues SET
            [Value] = '202122',
            LastUpdatedAt = @currentDateTime,
            LastUpdatedBy = @migrationUser
        WHERE FundingStreamId = @fundingStreamId
          AND SettingId = @academySchoolAcademicYearSettingId
    END
ELSE
	BEGIN
		INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
		VALUES (@fundingStreamId, @academySchoolAcademicYearSettingId, '202122', @currentDateTime, @currentDateTime, @migrationUser)
	END

-- Publication
IF NOT EXISTS (SELECT Id FROM Publications WHERE FundingStreamId IN (SELECT Id FROM FundingStreams WHERE FundingStreamCode = '1416'))
    BEGIN
		INSERT Publications (FundingStreamId, PublishedDate, FundingPeriodCode, CutOffDate, Description, Status, UIModelVersion, SpreadsheetModelVersion, CreatedAt, LastUpdatedAt, LastUpdatedBy)
		VALUES (@fundingStreamId, '2021-07-01 00:00:00.0000000', 'AS-2122', NULL, NULL, 2, NULL, NULL, @currentDateTime, @currentDateTime, @migrationUser)
    END

COMMIT TRAN T1;