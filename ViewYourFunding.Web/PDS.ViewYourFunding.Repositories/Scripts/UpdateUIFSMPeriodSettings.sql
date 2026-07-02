BEGIN TRAN T1;

DECLARE
    @fundingStreamId INT = 0,
    @financialYearSettingId INT,
    @academicYearSettingId INT,
    @currentDateTime DATETIME = GETDATE(),
    @migrationUser nvarchar(128) = 'Migration'

-- Get Ids
SELECT @fundingStreamId = Id FROM FundingStreams WHERE FundingStreamCode = 'UIFSM'
SELECT @financialYearSettingId = Id FROM Settings WHERE SettingName = 'FinancialYear'

IF(@fundingStreamId!=0)
BEGIN
-- Delete Financial year setting
IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @financialYearSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
       DELETE FROM SettingValues WHERE SettingId = @financialYearSettingId AND FundingStreamId = @fundingStreamId
    END


SELECT @academicYearSettingId = Id FROM Settings WHERE SettingName = 'AcademicYear'

IF NOT EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @academicYearSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
        INSERT SettingValues (FundingStreamId, SettingId, Value, CreatedAt, LastUpdatedAt, LastUpdatedBy)
        VALUES (@fundingStreamId, @academicYearSettingId, '202223', @currentDateTime, @currentDateTime, @migrationUser)
    END

    -- Add Publication
IF NOT EXISTS (SELECT Id FROM Publications WHERE FundingStreamId IN (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'UIFSM') and FundingPeriodCode = 'AY-2223')
    BEGIN
		INSERT Publications (FundingStreamId, PublishedDate, FundingPeriodCode, CutOffDate, Description, Status, UIModelVersion, SpreadsheetModelVersion, CreatedAt, LastUpdatedAt, LastUpdatedBy)
		VALUES (@fundingStreamId, '2022-07-01 00:00:00.0000000', 'AY-2223', NULL, NULL, 2, NULL, NULL, @currentDateTime, @currentDateTime, @migrationUser)
    END

    -- Delete Publications
IF EXISTS (SELECT Id FROM Publications WHERE FundingStreamId IN (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'UIFSM') and FundingPeriodCode = 'FY-2223')
    BEGIN
		DELETE FROM Publications 
        WHERE FundingStreamId IN (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'UIFSM') and FundingPeriodCode = 'FY-2223'
    END
END

COMMIT TRAN T1;
