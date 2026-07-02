BEGIN TRAN T1;

UPDATE [dbo].[FundingStreams]
   SET [RelevantForOrganisations_LoggedIn] = 0
      ,[RelevantForOrganisations_Public] = 0
      ,[RelevantForProviders_LoggedIn] = 0
      ,[RelevantForProviders_Public] = 0
      ,[RelevantForNational] = 0
      ,[FundingStreamCodePubliclyKnown] = 0
      ,[FundingStreamNameWithinSentence] = ''
 WHERE FundingStreamCode = 'DSG'

UPDATE [dbo].[FundingStreams]
   SET [RelevantForOrganisations_LoggedIn] = 0
      ,[RelevantForOrganisations_Public] = 0
      ,[RelevantForProviders_LoggedIn] = 0
      ,[RelevantForProviders_Public] = 0
      ,[RelevantForNational] = 0
      ,[FundingStreamCodePubliclyKnown] = 0
      ,[FundingStreamNameWithinSentence] = ''
 WHERE FundingStreamCode = 'PSG'

UPDATE [dbo].[FundingStreams]
   SET [RelevantForOrganisations_LoggedIn] = 0
      ,[RelevantForOrganisations_Public] = 0
      ,[RelevantForProviders_LoggedIn] = 0
      ,[RelevantForProviders_Public] = 0
      ,[RelevantForNational] = 0
      ,[FundingStreamCodePubliclyKnown] = 0
      ,[FundingStreamNameWithinSentence] = ''
 WHERE FundingStreamCode = 'GAG'

COMMIT TRAN T1;