BEGIN TRAN T1;

DECLARE
    @dsgId INT,
    @psgId INT,
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

SELECT	@dsgId = Id
FROM	[dbo].[FundingStreams]
WHERE	FundingStreamCode = 'DSG'

SELECT	@psgId = Id
FROM	[dbo].[FundingStreams]
WHERE	FundingStreamCode = 'PSG'

-- Update next all DSG Next Payments
UPDATE  [dbo].[NextPayments]
SET     FundingPeriodCode = 'FY-2021',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   FundingStreamId = @dsgId

-- Update next all PSG Next Payments
UPDATE  [dbo].[NextPayments]
SET     FundingPeriodCode = 'AY-1920',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   FundingStreamId = @psgId

COMMIT TRAN T1;