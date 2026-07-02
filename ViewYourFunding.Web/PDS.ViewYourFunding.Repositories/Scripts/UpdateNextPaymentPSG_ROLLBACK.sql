BEGIN TRAN T1;

INSERT INTO [dbo].[NextPayments]
           ([NextPaymentDate]
           ,[NextPaymentTypeId]
           ,[Active]
           ,[FundingStreamId]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy]
           ,[FundingPeriodCode])
     VALUES
           ('2022-05-04'
           ,(select Id from [dbo].[NextPaymentTypes] where [Description] = 'Academies')
           ,1
           ,(select Id from [dbo].[FundingStreams] where [FundingStreamCode] = 'PSG')
           ,GETDATE()
           ,GETDATE()
           ,'Migration'
           ,'AY-2122')

COMMIT TRAN T1;