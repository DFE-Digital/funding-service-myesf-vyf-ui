BEGIN TRAN T1;
DECLARE 
	@psgId INT,
	@dsgId INT,
      @cutOffDateSettingId INT,
      @nextAllocationPaymentDateSettingId INT,
	@nextPaymentDateMaintainedSettingId INT,
      @nextPaymentDateAcademiesSettingId INT,
	@nextPaymentDateNMSSSettingId INT,
	@currentDateTime DATETIME

SELECT	@currentDateTime = GETDATE()

SELECT	@psgId = Id
FROM	      [FundingStreams]
WHERE	      FundingStreamCode = 'PSG'

SELECT	@dsgId = Id
FROM	      [FundingStreams]
WHERE	      FundingStreamCode = 'DSG'

INSERT INTO [Settings]
           ([SettingName]
           ,[SettingDescription]
           ,[ValueDataType]
           ,[ValuesAreEditable])
     VALUES
           ('CutOffDate'
           ,'Configures the cut off published date for retrieving the allocations.'
           ,5
           ,1)

SELECT	@cutOffDateSettingId = Id
FROM	      [Settings]
WHERE	      SettingName = 'CutOffDate'

INSERT INTO [Settings]
           ([SettingName]
           ,[SettingDescription]
           ,[ValueDataType]
           ,[ValuesAreEditable])
     VALUES
           ('NextAllocationPaymentDate'
           ,'The next allocation payment date for the funding stream.'
           ,5
           ,1)

SELECT	@nextAllocationPaymentDateSettingId = Id
FROM	      [Settings]
WHERE	      SettingName = 'NextAllocationPaymentDate'

INSERT INTO [Settings]
           ([SettingName]
           ,[SettingDescription]
           ,[ValueDataType]
           ,[ValuesAreEditable])
     VALUES
           ('NextPaymentDateMaintained'
           ,'The next allocation payment date for maintained schools for the funding stream.'
           ,5
           ,1)

SELECT	@nextPaymentDateMaintainedSettingId = Id
FROM	      [Settings]
WHERE	      SettingName = 'NextPaymentDateMaintained'

INSERT INTO [Settings]
           ([SettingName]
           ,[SettingDescription]
           ,[ValueDataType]
           ,[ValuesAreEditable])
     VALUES
           ('NextPaymentDateAcademies'
           ,'The next allocation payment date for academies for the funding stream.'
           ,5
           ,1)

SELECT	@nextPaymentDateAcademiesSettingId = Id
FROM	      [Settings]
WHERE	      SettingName = 'NextPaymentDateAcademies'

INSERT INTO [Settings]
           ([SettingName]
           ,[SettingDescription]
           ,[ValueDataType]
           ,[ValuesAreEditable])
     VALUES
           ('NextPaymentDateNMSS'
           ,'The next allocation payment date for NMSS for the funding stream.'
           ,5
           ,1)

SELECT	@nextPaymentDateNMSSSettingId = Id
FROM	      [Settings]
WHERE	      SettingName = 'NextPaymentDateNMSS'

INSERT INTO [SettingValues]
           ([FundingStreamId]
           ,[SettingId]
           ,[SettingValue]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           (@dsgId
           ,@cutOffDateSettingId
           ,'01/05/2020'
           ,@currentDateTime
           ,@currentDateTime
           ,'SYSTEM')

INSERT INTO [SettingValues]
           ([FundingStreamId]
           ,[SettingId]
           ,[SettingValue]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           (@psgId
           ,@cutOffDateSettingId
           ,'01/05/2020'
           ,@currentDateTime
           ,@currentDateTime
           ,'SYSTEM')

INSERT INTO [SettingValues]
           ([FundingStreamId]
           ,[SettingId]
           ,[SettingValue]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           (@dsgId
           ,@nextAllocationPaymentDateSettingId
           ,'03/04/2020'
           ,@currentDateTime
           ,@currentDateTime
           ,'SYSTEM')


INSERT INTO [SettingValues]
           ([FundingStreamId]
           ,[SettingId]
           ,[SettingValue]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           (@psgId
           ,@nextPaymentDateAcademiesSettingId
           ,'01/05/2020'
           ,@currentDateTime
           ,@currentDateTime
           ,'SYSTEM')

INSERT INTO [SettingValues]
           ([FundingStreamId]
           ,[SettingId]
           ,[SettingValue]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           (@psgId
           ,@nextPaymentDateMaintainedSettingId
           ,'30/04/2020'
           ,@currentDateTime
           ,@currentDateTime
           ,'SYSTEM')

INSERT INTO [SettingValues]
           ([FundingStreamId]
           ,[SettingId]
           ,[SettingValue]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           (@psgId
           ,@nextPaymentDateNMSSSettingId
           ,'01/05/2020'
           ,@currentDateTime
           ,@currentDateTime
           ,'SYSTEM')

COMMIT TRAN T1;