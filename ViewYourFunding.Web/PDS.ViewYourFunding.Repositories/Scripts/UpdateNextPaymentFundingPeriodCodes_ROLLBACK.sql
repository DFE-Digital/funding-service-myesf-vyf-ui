BEGIN TRAN T1;

DECLARE
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()


-- Update next all Next Payments
UPDATE  [dbo].[NextPayments]
SET     FundingPeriodCode = NULL,
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser

COMMIT TRAN T1;