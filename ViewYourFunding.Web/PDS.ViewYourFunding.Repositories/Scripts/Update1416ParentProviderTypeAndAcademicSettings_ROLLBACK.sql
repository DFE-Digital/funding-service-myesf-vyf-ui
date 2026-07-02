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
SELECT @academicYearSettingId = Id FROM Settings WHERE SettingName = 'AcademicYear'

-- Update parent provider type setting
IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @parentProviderTypeSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        UPDATE SettingValues SET
            [Value] = 'LocalAuthority',
            LastUpdatedAt = @currentDateTime,
            LastUpdatedBy = @migrationUser
        WHERE FundingStreamId = @fundingStreamId
          AND SettingId = @parentProviderTypeSettingId
    END
ELSE
    BEGIN
        INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
        VALUES (@fundingStreamId, @parentProviderTypeSettingId, 'LocalAuthority', @currentDateTime, @currentDateTime, @migrationUser)
    END


-- Add back AcademyAndSchoolAcademicYear setting
IF NOT EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @academyAndSchoolAcademicYearSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
        VALUES (@fundingStreamId, @academyAndSchoolAcademicYearSettingId, '202122', @currentDateTime, @currentDateTime, @migrationUser)
    END

 -- Remove AcademicYear setting
IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @academicYearSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
       DELETE FROM SettingValues WHERE SettingId = @academicYearSettingId AND FundingStreamId = @fundingStreamId
    END


COMMIT TRAN T1;
