BEGIN TRAN T1;

UPDATE  [dbo].[FundingStreams]
SET     [RelevantForProviders_LoggedIn] = 1,
		[HistoryIndependentOfPublications] = 1
WHERE   FundingStreamCode in ('1619')

COMMIT TRAN T1;