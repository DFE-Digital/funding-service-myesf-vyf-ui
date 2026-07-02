BEGIN TRAN T1;
DECLARE
	@currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

TRUNCATE TABLE [dbo].[GlobalSettings]

 -- Inserts demo settings
INSERT INTO [dbo].[GlobalSettings]
           ([Type]
           ,[Value]
           ,[CreatedAt]
           ,[UpdatedAt]
           ,[Description]
           ,[EditType]
           ,[ReadOnly]
           ,[LastUpdatedBy])
     VALUES
           (6
           ,'23'
           ,@currentDateTime
           ,@currentDateTime
           ,'Demo Int type global setting'
           ,1
           ,0
           ,@migrationUser)

INSERT INTO [dbo].[GlobalSettings]
           ([Type]
           ,[Value]
           ,[CreatedAt]
           ,[UpdatedAt]
           ,[Description]
           ,[EditType]
           ,[ReadOnly]
           ,[LastUpdatedBy])
     VALUES
           (7
           ,'18:45'
           ,@currentDateTime
           ,@currentDateTime
           ,'Demo Time global setting'
           ,4
           ,0
           ,@migrationUser)

INSERT INTO [dbo].[GlobalSettings]
           ([Type]
           ,[Value]
           ,[CreatedAt]
           ,[UpdatedAt]
           ,[Description]
           ,[EditType]
           ,[ReadOnly]
           ,[LastUpdatedBy])
     VALUES
           (8
           ,'12/04/2020'
           ,@currentDateTime
           ,@currentDateTime
           ,'Demo date global setting'
           ,5
           ,0
           ,@migrationUser)

INSERT INTO [dbo].[GlobalSettings]
           ([Type]
           ,[Value]
           ,[CreatedAt]
           ,[UpdatedAt]
           ,[Description]
           ,[EditType]
           ,[ReadOnly]
           ,[LastUpdatedBy])
     VALUES
           (9
           ,'09/04/2020 18:21'
           ,@currentDateTime
           ,@currentDateTime
           ,'Demo date time global setting'
           ,2
           ,0
           ,@migrationUser)

           -- Inserts the incorrect duplicate
INSERT INTO [dbo].[GlobalSettings]
           ([Type]
           ,[Value]
           ,[CreatedAt]
           ,[UpdatedAt]
           ,[Description]
           ,[EditType]
           ,[ReadOnly]
           ,[LastUpdatedBy])
     VALUES
           (5
           ,'/single-funding-statement/latest/logged-in-provider-statement'
           ,@currentDateTime
           ,@currentDateTime
           ,'Url for logged-in provider view'
           ,0
           ,0
           ,@migrationUser)

           
INSERT INTO [dbo].[GlobalSettings]
           ([Type]
           ,[Value]
           ,[CreatedAt]
           ,[UpdatedAt]
           ,[Description]
           ,[EditType]
           ,[ReadOnly]
           ,[LastUpdatedBy])
     VALUES
           (11
           ,'false'
           ,@currentDateTime
           ,@currentDateTime
           ,'Url for logged-in provider view'
           ,0
           ,0
           ,@migrationUser)
           
           --Insert Settings that were renamed
INSERT INTO [dbo].[GlobalSettings]
           ([Type]
           ,[Value]
           ,[CreatedAt]
           ,[UpdatedAt]
           ,[Description]
           ,[EditType]
           ,[ReadOnly]
           ,[LastUpdatedBy])
     VALUES
           (5
           ,'/single-funding-statement/latest/admin/home'
           ,@currentDateTime
           ,@currentDateTime
           ,'Url for logged-in admin view'
           ,0
           ,0
           ,@migrationUser)

INSERT INTO [dbo].[GlobalSettings]
           ([Type]
           ,[Value]
           ,[CreatedAt]
           ,[UpdatedAt]
           ,[Description]
           ,[EditType]
           ,[ReadOnly]
           ,[LastUpdatedBy])
     VALUES
           (4
           ,'/single-funding-statement/latest/start'
           ,@currentDateTime
           ,@currentDateTime
           ,'Url for external view'
           ,0
           ,0
           ,@migrationUser)

INSERT INTO [dbo].[GlobalSettings]
           ([Type]
           ,[Value]
           ,[CreatedAt]
           ,[UpdatedAt]
           ,[Description]
           ,[EditType]
           ,[ReadOnly]
           ,[LastUpdatedBy])
     VALUES
           (4
           ,'FALSE'
           ,@currentDateTime
           ,@currentDateTime
           ,'Is View Your Funding logged in view area available?'
           ,3
           ,0
           ,@migrationUser)

INSERT INTO [dbo].[GlobalSettings]
           ([Type]
           ,[Value]
           ,[CreatedAt]
           ,[UpdatedAt]
           ,[Description]
           ,[EditType]
           ,[ReadOnly]
           ,[LastUpdatedBy])
     VALUES
           (4
           ,'FALSE'
           ,@currentDateTime
           ,@currentDateTime
           ,'Is View Your Funding External area available?'
           ,3
           ,0
           ,@migrationUser)

COMMIT TRAN T1;