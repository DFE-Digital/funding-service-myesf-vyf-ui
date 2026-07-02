BEGIN TRAN T1;

DECLARE
    @fundingStreamId INT,
    @parentProviderTypeSettingId INT,
    @academyAndSchoolAcademicYearSettingId INT,
    @academicYearSettingId INT,
    @currentDateTime DATETIME = GETDATE(),
    @migrationUser nvarchar(128) = 'Migration'

-- Get Ids
SELECT @fundingStreamId = Id FROM FundingStreams WHERE FundingStreamCode = '1416'
SELECT @parentProviderTypeSettingId = Id FROM Settings WHERE SettingName = 'ParentProviderType'
SELECT @academyAndSchoolAcademicYearSettingId = Id FROM Settings WHERE SettingName = 'AcademyAndSchoolAcademicYear'

-- Update parent provider type setting
IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @parentProviderTypeSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        UPDATE SettingValues SET
            [Value] = 'Provider',
            LastUpdatedAt = @currentDateTime,
            LastUpdatedBy = @migrationUser
        WHERE FundingStreamId = @fundingStreamId
          AND SettingId = @parentProviderTypeSettingId
    END
ELSE
    BEGIN
        INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
        VALUES (@fundingStreamId, @parentProviderTypeSettingId, 'Provider', @currentDateTime, @currentDateTime, @migrationUser)
    END


-- Delete AcademyAndSchoolAcademicYear setting
IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @academyAndSchoolAcademicYearSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
       DELETE FROM SettingValues WHERE SettingId = @academyAndSchoolAcademicYearSettingId AND FundingStreamId = @fundingStreamId
    END


 -- Add AcademicYear setting

 if NOT EXISTS (SELECT Id FROM Settings WHERE SettingName = 'AcademicYear')
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
           ('AcademicYear',
		   'The academic year of the allocations that should be shown, e.g. 201819.',
		   0,
		   1,
		   @currentDateTime,
		   @currentDateTime,
		   @migrationUser)
END

SELECT @academicYearSettingId = Id FROM Settings WHERE SettingName = 'AcademicYear'

IF NOT EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @academicYearSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
        VALUES (@fundingStreamId, @academicYearSettingId, '202122', @currentDateTime, @currentDateTime, @migrationUser)
    END

    -- Publication
IF NOT EXISTS (SELECT Id FROM Publications WHERE FundingStreamId IN (SELECT Id FROM FundingStreams WHERE FundingStreamCode = '1416') and FundingPeriodCode = 'AY-2122')
    BEGIN
		INSERT Publications (FundingStreamId, PublishedDate, FundingPeriodCode, CutOffDate, Description, Status, UIModelVersion, SpreadsheetModelVersion, CreatedAt, LastUpdatedAt, LastUpdatedBy)
		VALUES (@fundingStreamId, '2022-07-01 00:00:00.0000000', 'AY-2122', NULL, NULL, 2, NULL, NULL, @currentDateTime, @currentDateTime, @migrationUser)
    END



COMMIT TRAN T1;
