BEGIN TRAN T1;

DECLARE
    @fundingStreamId INT,
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

SELECT	@fundingStreamId = Id
FROM	[dbo].[FundingStreams]
WHERE	FundingStreamCode = '1619'

-- Update GAG FundingStream
UPDATE  [dbo].[FundingStreams]
SET     RelevantForOrganisations_LoggedIn = 0,
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @fundingStreamId

COMMIT TRAN T1;