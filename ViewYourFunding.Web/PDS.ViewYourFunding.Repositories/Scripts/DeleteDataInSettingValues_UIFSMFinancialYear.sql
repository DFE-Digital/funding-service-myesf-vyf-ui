BEGIN TRAN T1;

DECLARE
    @fundingStreamId INT,
    @financialYearSettingId INT

-- Get Ids
SELECT @fundingStreamId = Id FROM FundingStreams WHERE FundingStreamCode = 'UIFSM'
SELECT @financialYearSettingId = Id FROM dbo.Settings WHERE SettingName = 'FinancialYear'

-- Delete Financial year setting
IF EXISTS (SELECT Id FROM SettingValues WHERE SettingId = @financialYearSettingId AND FundingStreamId = @fundingStreamId)
    BEGIN
       DELETE FROM SettingValues WHERE SettingId = @financialYearSettingId AND FundingStreamId = @fundingStreamId
    END

COMMIT TRAN T1;
