BEGIN TRANSACTION

DELETE Publications WHERE FundingStreamId in (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'PPG')
DELETE NextPayments WHERE NextPaymentTypeId in (SELECT Id FROM NextPaymentTypes WHERE FundingStreamId in (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'PPG'))
DELETE NextPaymentTypes WHERE FundingStreamId in (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'PPG')
DELETE SettingValues WHERE FundingStreamId in (SELECT Id FROM FundingStreams WHERE FundingStreamCode = 'PPG')
DELETE FundingStreams WHERE FundingStreamCode = 'PPG'

COMMIT TRANSACTION