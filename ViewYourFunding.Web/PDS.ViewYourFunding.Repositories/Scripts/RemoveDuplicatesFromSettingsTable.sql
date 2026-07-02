BEGIN TRAN T1;

DELETE FROM  [dbo].[SettingValues] 
GO

DELETE FROM [dbo].[Settings] 
GO

COMMIT TRAN T1;