BEGIN TRAN T1;

UPDATE  [dbo].[FundingStreams]
SET     [RelevantForProviders_LoggedIn] = 0,
		[HistoryIndependentOfPublications] = 0
WHERE   FundingStreamCode in ('1619')

COMMIT TRAN T1;