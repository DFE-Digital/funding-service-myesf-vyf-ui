BEGIN TRANSACTION

DECLARE
	@currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

INSERT INTO FundingStreams 
([FundingStreamCode], [FundingStreamName], [Active], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy], [DeletedAt], [FundingStreamCodePubliclyKnown], [FundingStreamNameWithinSentence], [RelevantForNational], [RelevantForOrganisations_LoggedIn], [RelevantForOrganisations_Public], [RelevantForProviders_LoggedIn], [RelevantForProviders_Public], [HistoryIndependentOfPublications])
VALUES ('PPG', 'Pupil premium', 1, @currentDateTime, @currentDateTime, @migrationUser, null, 0, null, 0, 1, 0, 1, 0, 1)

COMMIT TRANSACTION