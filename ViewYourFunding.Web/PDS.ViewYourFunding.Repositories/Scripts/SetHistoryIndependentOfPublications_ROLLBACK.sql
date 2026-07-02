BEGIN TRAN T1;

DECLARE
    @gagId INT,
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

SELECT	@gagId = Id
FROM	[dbo].[FundingStreams]
WHERE	FundingStreamCode = 'GAG'

-- Update GAG FundingStream
UPDATE  [dbo].[FundingStreams]
SET     HistoryIndependentOfPublications = 0,
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @gagId

COMMIT TRAN T1;