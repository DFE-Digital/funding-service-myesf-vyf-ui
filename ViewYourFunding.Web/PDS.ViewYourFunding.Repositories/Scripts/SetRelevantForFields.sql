BEGIN TRAN T1;

UPDATE [dbo].[FundingStreams]
   SET [RelevantForOrganisations_LoggedIn] = 0
      ,[RelevantForOrganisations_Public] = 1
      ,[RelevantForProviders_LoggedIn] = 0
      ,[RelevantForProviders_Public] = 0
      ,[RelevantForNational] = 1
      ,[FundingStreamCodePubliclyKnown] = 1
      ,[FundingStreamNameWithinSentence] = 'dedicated schools grant'
 WHERE FundingStreamCode = 'DSG'

UPDATE [dbo].[FundingStreams]
   SET [RelevantForOrganisations_LoggedIn] = 1
      ,[RelevantForOrganisations_Public] = 1
      ,[RelevantForProviders_LoggedIn] = 1
      ,[RelevantForProviders_Public] = 1
      ,[RelevantForNational] = 1
      ,[FundingStreamCodePubliclyKnown] = 0
      ,[FundingStreamNameWithinSentence] = 'PE and sport premium'
 WHERE FundingStreamCode = 'PSG'

UPDATE [dbo].[FundingStreams]
   SET [RelevantForOrganisations_LoggedIn] = 0
      ,[RelevantForOrganisations_Public] = 0
      ,[RelevantForProviders_LoggedIn] = 1
      ,[RelevantForProviders_Public] = 0
      ,[RelevantForNational] = 0
      ,[FundingStreamCodePubliclyKnown] = 0
      ,[FundingStreamNameWithinSentence] = 'general annual grant'
 WHERE FundingStreamCode = 'GAG'

COMMIT TRAN T1;