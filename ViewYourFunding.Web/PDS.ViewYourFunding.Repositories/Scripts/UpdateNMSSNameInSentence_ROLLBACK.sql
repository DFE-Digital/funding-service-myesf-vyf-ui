BEGIN TRAN T1;

DECLARE        
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

UPDATE  [dbo].[FundingStreams]
SET     FundingStreamNameWithinSentence = 'Non maintained special school funding',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   FundingStreamCode = 'NMSS'

COMMIT TRAN T1;