BEGIN TRAN T1;

DECLARE
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

-- Update 1619 FundingStream
UPDATE  [dbo].[FundingStreams]
SET     FundingStreamName = '16 to 19',
        FundingStreamNameWithinSentence = '16 to 19',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   FundingStreamCode = '1619'

COMMIT TRAN T1;