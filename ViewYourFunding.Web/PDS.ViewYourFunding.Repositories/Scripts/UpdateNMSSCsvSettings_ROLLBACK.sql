BEGIN TRAN T1;

DECLARE
    @fundingStreamId INT,
    @fileTypeSettingId INT,
    @fileSizeSettingId INT,
    @currentDateTime DATETIME = GETDATE(),
    @migrationUser nvarchar(128) = 'Migration'

SELECT @fundingStreamId = Id FROM FundingStreams WHERE FundingStreamCode = 'NMSS'

SELECT @fileTypeSettingId = Id FROM Settings WHERE SettingName = 'FundingDocumentFileType'

SELECT @fileSizeSettingId = Id FROM Settings WHERE SettingName = 'ProviderDownloadSizeInBytes'

DELETE FROM SettingValues WHERE FundingStreamId = @fundingStreamId AND SettingId IN (@fileTypeSettingId, @fileSizeSettingId)

COMMIT TRAN T1;