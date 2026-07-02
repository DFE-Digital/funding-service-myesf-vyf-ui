BEGIN TRANSACTION

DELETE Publications WHERE FundingStreamId IN (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'PNAAC')
DELETE NextPayments WHERE NextPaymentTypeId IN (SELECT Id FROM NextPaymentTypes WHERE FundingStreamId = (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'PNAAC'))
DELETE NextPaymentTypes WHERE FundingStreamId IN (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'PNAAC')
DELETE SettingValues WHERE FundingStreamId IN (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'PNAAC')
DELETE FundingStreams WHERE FundingStreamCode = 'PNAAC'

COMMIT TRANSACTION