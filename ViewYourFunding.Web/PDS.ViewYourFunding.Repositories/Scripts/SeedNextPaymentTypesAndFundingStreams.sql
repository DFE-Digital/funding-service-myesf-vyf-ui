DECLARE
    @now DATETIME = GETDATE(),
    @migrationUser nvarchar(128) = 'Migration',
    @fundingPeriod nvarchar(max) = 'AY-2021'

BEGIN TRAN T1;

IF NOT EXISTS (SELECT *  FROM [dbo].[FundingStreams] where [FundingStreamCode] = 'PSG')
INSERT INTO [dbo].[FundingStreams]
           ([FundingStreamCode]
           ,[FundingStreamName]
           ,[Active]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy]
           ,[DeletedAt]
           ,[FundingStreamCodePubliclyKnown]
           ,[FundingStreamNameWithinSentence]
           ,[RelevantForNational]
           ,[RelevantForOrganisations_LoggedIn]
           ,[RelevantForOrganisations_Public]
           ,[RelevantForProviders_LoggedIn]
           ,[RelevantForProviders_Public]
           ,[HistoryIndependentOfPublications])
     VALUES
           ('PSG'
           ,'PE and sport premium'
           ,1
           ,@now
           ,@now
           ,@migrationUser
           ,null
           ,0
           ,null
           ,1
           ,1
           ,1
           ,1
           ,1
           ,0)

IF NOT EXISTS (SELECT *  FROM [dbo].[FundingStreams] where [FundingStreamCode] = 'DSG')
INSERT INTO [dbo].[FundingStreams]
           ([FundingStreamCode]
           ,[FundingStreamName]
           ,[Active]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy]
           ,[DeletedAt]
           ,[FundingStreamCodePubliclyKnown]
           ,[FundingStreamNameWithinSentence]
           ,[RelevantForNational]
           ,[RelevantForOrganisations_LoggedIn]
           ,[RelevantForOrganisations_Public]
           ,[RelevantForProviders_LoggedIn]
           ,[RelevantForProviders_Public]
           ,[HistoryIndependentOfPublications])
     VALUES
           ('DSG'
           ,'Dedicated schools grant'
           ,1
           ,@now
           ,@now
           ,@migrationUser
           ,null
           ,1
           ,'dedicated schools grant'
           ,1
           ,0
           ,1
           ,0
           ,0
           ,0)  
COMMIT TRAN T1;

BEGIN TRAN T2;

DECLARE
    @dsgId int = (Select Id from [dbo].[FundingStreams] where [FundingStreamCode] = 'DSG'),
    @psgId int = (Select Id from [dbo].[FundingStreams] where [FundingStreamCode] = 'PSG')

IF NOT EXISTS (SELECT *  FROM [dbo].[NextPaymentTypes] where TypeCode = 'FS')
INSERT INTO [dbo].[NextPaymentTypes]
           ([TypeCode]
           ,[Description]
           ,[FundingStreamId]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           ('FS'
           ,'Funding Specific'
           ,@dsgId
           ,@now
           ,@now
           ,@migrationUser)
IF NOT EXISTS (SELECT *  FROM [dbo].[NextPaymentTypes] where TypeCode = 'NMSS')
INSERT INTO [dbo].[NextPaymentTypes]
           ([TypeCode]
           ,[Description]
           ,[FundingStreamId]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           ('NMSS'
           ,'Non maintained schools'
           ,@psgId
           ,@now
           ,@now
           ,@migrationUser)
IF NOT EXISTS (SELECT *  FROM [dbo].[NextPaymentTypes] where TypeCode = 'AD')
INSERT INTO [dbo].[NextPaymentTypes]
           ([TypeCode]
           ,[Description]
           ,[FundingStreamId]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           ('AD'
           ,'Academies'
           ,@psgId
           ,@now
           ,@now
           ,@migrationUser)
IF NOT EXISTS (SELECT *  FROM [dbo].[NextPaymentTypes] where TypeCode = 'MS')
INSERT INTO [dbo].[NextPaymentTypes]
           ([TypeCode]
           ,[Description]
           ,[FundingStreamId]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           ('MS'
           ,'Maintained schools'
           ,@psgId
           ,@now
           ,@now
           ,@migrationUser)
COMMIT TRAN T2;