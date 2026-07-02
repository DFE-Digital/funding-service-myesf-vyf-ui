BEGIN TRAN T1;

DECLARE
    @dsgId INT,
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

SELECT	@dsgId = Id
FROM	[dbo].[FundingStreams]
WHERE	FundingStreamCode = 'DSG'


-- Update next all Next Payments
UPDATE  [dbo].[NextPayments]
SET     FundingPeriodCode = NULL,
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   FundingStreamId = @dsgId

COMMIT TRAN T1;