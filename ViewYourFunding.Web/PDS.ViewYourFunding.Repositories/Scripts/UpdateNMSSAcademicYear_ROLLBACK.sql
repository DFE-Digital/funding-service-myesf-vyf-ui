BEGIN TRAN T1;

DECLARE
    @fundingStreamId INT,
    @academicYearSettingId INT,
    @academyAndSchoolAcademicYearSettingId INT,
    @currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

SELECT	@fundingStreamId = Id
FROM	[dbo].[FundingStreams]
WHERE	FundingStreamCode = 'NMSS'

SELECT @academicYearSettingId =  Id
FROM [dbo].[Settings]
WHERE SettingName = 'AcademicYear'  

SELECT @academyAndSchoolAcademicYearSettingId =  Id
FROM [dbo].[Settings]
WHERE SettingName = 'AcademyAndSchoolAcademicYear' 

-- DELETE  NMSS AcademicYear setting if it exists

IF EXISTS(SELECT 1 FROM [dbo].[SettingValues] WHERE FundingStreamId = @fundingStreamId AND SettingId = @academicYearSettingId)
BEGIN
DELETE FROM [dbo].[SettingValues]
WHERE   FundingStreamId = @fundingStreamId AND SettingId = @academicYearSettingId
END

-- INSERT  NMSS AcademyAndSchoolAcademicYear setting 
IF NOT EXISTS(SELECT 1 FROM [dbo].[SettingValues] WHERE FundingStreamId = @fundingStreamId AND SettingId = @academyAndSchoolAcademicYearSettingId)
BEGIN
INSERT INTO [dbo].[SettingValues]
           ([FundingStreamId]
           ,[SettingId]
           ,[Value]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
VALUES
           (@fundingStreamId
           ,@academyAndSchoolAcademicYearSettingId
           ,'202122'
           ,@currentDateTime
           ,@currentDateTime
           ,@migrationUser)
END

COMMIT TRAN T1;