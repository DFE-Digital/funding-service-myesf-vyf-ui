BEGIN TRAN T1;
DECLARE
	@currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

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
           (1
           ,'TRUE'
           ,@currentDateTime
           ,@currentDateTime
           ,'Use secure OAuth View Your Funding API?'
           ,3
           ,0
           ,@migrationUser)

COMMIT TRAN T1;