BEGIN TRAN T1;

DECLARE
    @gagId INT,
    @psgId INT,
    @dsgId INT,
    @sixteenId INT,
    @nmssId INT,
    @ppgId INT,
    @fourteenId INT,
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

SELECT	@psgId = Id         FROM	[dbo].[FundingStreams] WHERE	FundingStreamCode = 'PSG'
SELECT	@dsgId = Id         FROM	[dbo].[FundingStreams] WHERE	FundingStreamCode = 'DSG'
SELECT	@gagId = Id         FROM	[dbo].[FundingStreams] WHERE	FundingStreamCode = 'GAG'
SELECT	@sixteenId = Id     FROM	[dbo].[FundingStreams] WHERE	FundingStreamCode = '1619'
SELECT	@nmssId = Id        FROM	[dbo].[FundingStreams] WHERE	FundingStreamCode = 'NMSS'
SELECT	@ppgId = Id         FROM	[dbo].[FundingStreams] WHERE	FundingStreamCode = 'PPG'
SELECT	@fourteenId = Id    FROM	[dbo].[FundingStreams] WHERE	FundingStreamCode = '1416'

UPDATE  [dbo].[FundingStreams]
SET     FundingStreamBusinessAllocationName = 'PE and sport premium grant (PSG)',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @psgId

UPDATE  [dbo].[FundingStreams]
SET     FundingStreamBusinessAllocationName = 'Dedicated schools grant (DSG)',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @dsgId

UPDATE  [dbo].[FundingStreams]
SET     FundingStreamBusinessAllocationName = 'General annual grant (GAG)',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @gagId

UPDATE  [dbo].[FundingStreams]
SET     FundingStreamBusinessAllocationName = '16 to 19 funding (16-19)',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @sixteenId

UPDATE  [dbo].[FundingStreams]
SET     FundingStreamBusinessAllocationName = 'Non-maintained special schools (NMSS)',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @nmssId

UPDATE  [dbo].[FundingStreams]
SET     FundingStreamBusinessAllocationName = 'Pupil premium grant (PPG)',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @ppgId

UPDATE  [dbo].[FundingStreams]
SET     FundingStreamBusinessAllocationName = '14 to 16 funding (14-16)',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @fourteenId


COMMIT TRAN T1;