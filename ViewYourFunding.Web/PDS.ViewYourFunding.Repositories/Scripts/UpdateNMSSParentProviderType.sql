BEGIN TRAN T1;

DECLARE
    @fundingStreamId INT,
    @settingId INT,
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

SELECT	@fundingStreamId = Id
FROM	[dbo].[FundingStreams]
WHERE	FundingStreamCode = 'NMSS'

SELECT @settingId
FROM [dbo].[Settings]
WHERE SettingName = 'ParentProviderType'  

-- Update NMSS ParentProviderType setting
UPDATE  [dbo].[SettingValues]
SET     [Value] = 'LocalAuthority',
        LastUpdatedAt  = @currentDateTime,
        LastUpdatedBy  = @migrationUser
WHERE   FundingStreamId = @fundingStreamId AND SettingId = @settingId

COMMIT TRAN T1;