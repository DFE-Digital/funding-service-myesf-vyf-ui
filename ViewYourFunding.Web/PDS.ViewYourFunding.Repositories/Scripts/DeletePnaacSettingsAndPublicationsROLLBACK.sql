BEGIN TRANSACTION

DECLARE
	@currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

INSERT INTO FundingStreams 
([FundingStreamCode], [FundingStreamName], [Active], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy], [DeletedAt], [FundingStreamCodePubliclyKnown], [FundingStreamNameWithinSentence], [RelevantForNational], [RelevantForOrganisations_LoggedIn], [RelevantForOrganisations_Public], [RelevantForProviders_LoggedIn], [RelevantForProviders_Public], [HistoryIndependentOfPublications])
VALUES ('PNAAC', 'Main PNA', 1, @currentDateTime, @currentDateTime, @migrationUser, null, 0, null, 0, 0, 0, 0, 0, 1)

COMMIT TRANSACTION