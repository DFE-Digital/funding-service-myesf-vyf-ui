DECLARE        
    @currentDateTime DATETIME,
    @user nvarchar(128) = 'Migration',
	@settingCount int

SELECT	@currentDateTime = GETDATE()
SELECT @settingCount = COUNT(*) FROM [dbo].[Settings] WHERE [SettingName] = 'UseAutoPull'

IF @settingCount = 0
	INSERT INTO [dbo].[Settings]
	VALUES('UseAutoPull', 'Determines whether to use the auto pull functionality.',3,1,@currentDateTime,@currentDateTime,@user)