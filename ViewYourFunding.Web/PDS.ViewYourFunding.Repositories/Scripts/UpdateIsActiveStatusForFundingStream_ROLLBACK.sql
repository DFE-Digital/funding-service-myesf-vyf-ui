BEGIN TRAN T1;

DECLARE
    @dsgId INT,
    @psgId INT,
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

SELECT	@dsgId = Id
FROM	[dbo].[FundingStreams]
WHERE	FundingStreamCode = 'DSG'

SELECT	@psgId = Id
FROM	[dbo].[FundingStreams]
WHERE	FundingStreamCode = 'PSG'

-- Update DSG FundingStream
UPDATE  [dbo].[FundingStreams]
SET     Active = 0,
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @dsgId

-- Update PSG FundingStream
UPDATE  [dbo].[FundingStreams]
SET     Active = 0,
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @psgId

COMMIT TRAN T1;