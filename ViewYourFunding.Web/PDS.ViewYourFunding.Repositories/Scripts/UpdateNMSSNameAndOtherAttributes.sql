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

UPDATE  [dbo].[FundingStreams]
SET     [RelevantForProviders_LoggedIn] = 1,
		FundingStreamName = 'Non maintained special school funding',
        FundingStreamNameWithinSentence = 'Non maintained special school funding',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @fundingStreamId

SELECT @settingId = Id
FROM [dbo].[Settings]
WHERE SettingName = 'ParentProviderType'  

-- Update NMSS ParentProviderType setting
UPDATE  [dbo].[SettingValues]
SET     [Value] = 'LocalAuthority',
        LastUpdatedAt  = @currentDateTime,
        LastUpdatedBy  = @migrationUser
WHERE   FundingStreamId = @fundingStreamId AND SettingId = @settingId

COMMIT TRAN T1;