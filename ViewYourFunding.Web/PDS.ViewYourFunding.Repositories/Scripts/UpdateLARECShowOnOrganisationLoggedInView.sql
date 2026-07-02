BEGIN TRAN T1;

DECLARE        
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

UPDATE  [dbo].[FundingStreams]
SET     RelevantForOrganisations_LoggedIn = 1,
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   FundingStreamCode = 'LAREC'

COMMIT TRAN T1;