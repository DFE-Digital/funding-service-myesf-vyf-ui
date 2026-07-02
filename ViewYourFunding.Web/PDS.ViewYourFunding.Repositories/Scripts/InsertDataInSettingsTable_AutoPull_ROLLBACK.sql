DECLARE        
	@autoPullSettingID int

SELECT	@autoPullSettingID = [Id] FROM [Settings] WHERE [SettingName] = 'UseAutoPull'

DELETE FROM [dbo].[SettingValues]
WHERE [SettingId] = @autoPullSettingID

DELETE FROM [dbo].[Settings]
WHERE [Id] = @autoPullSettingID