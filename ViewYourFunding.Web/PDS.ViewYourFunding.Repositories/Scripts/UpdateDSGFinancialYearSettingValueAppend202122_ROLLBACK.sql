BEGIN TRAN T1;

DECLARE
    @dsgId INT,
    @financialYearId INT,
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

SELECT	@dsgId = Id
FROM	[dbo].[FundingStreams]
WHERE	FundingStreamCode = 'DSG'

SELECT  @financialYearId = Id
FROM    [dbo].[Settings]
WHERE   SettingName = 'FinancialYear'

-- Reset financial year setting for DSG to '202021'
UPDATE  [dbo].[SettingValues]
SET     [Value] = '202021',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   FundingStreamId = @dsgId
AND     SettingId = @financialYearId

COMMIT TRAN T1;