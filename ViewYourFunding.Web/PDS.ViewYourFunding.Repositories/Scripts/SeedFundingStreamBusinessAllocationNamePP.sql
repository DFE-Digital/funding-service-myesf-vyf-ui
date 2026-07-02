BEGIN TRAN T1;

DECLARE
    @ppId INT,
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

SELECT	@ppId = Id  FROM [dbo].[FundingStreams] WHERE	FundingStreamCode = 'PP'


UPDATE  [dbo].[FundingStreams]
SET     FundingStreamBusinessAllocationName = 'Pupil premium grant (PPG)',
        LastUpdatedAt = @currentDateTime,
        LastUpdatedBy = @migrationUser
WHERE   Id = @ppId

COMMIT TRAN T1;