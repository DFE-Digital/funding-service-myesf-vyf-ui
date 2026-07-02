BEGIN TRAN T1;

DELETE
  FROM [dbo].[NextPayments]
  where NextPaymentTypeId = (select ID from [dbo].[NextPaymentTypes] where [Description] = 'Academies')
  and FundingPeriodCode = 'AY-2122'
  and NextPaymentDate = '2022-05-04'
  and [FundingStreamId] = (select ID from [dbo].[FundingStreams] where [FundingStreamCode] = 'PSG')

COMMIT TRAN T1;