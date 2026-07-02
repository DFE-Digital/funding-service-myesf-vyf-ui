BEGIN TRAN T1;

DECLARE        
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

UPDATE  [dbo].[FundingStreams]
SET     RelevantForProviders_LoggedIn = 0,
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   FundingStreamCode = 'PNA'

COMMIT TRAN T1;