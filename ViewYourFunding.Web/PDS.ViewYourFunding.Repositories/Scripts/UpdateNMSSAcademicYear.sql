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

if NOT EXISTS (SELECT Id FROM Settings WHERE SettingName = 'AcademicYear')
BEGIN
		INSERT INTO [Settings]
           ([SettingName]
           ,[SettingDescription]
           ,[ValueDataType]
           ,[ValuesAreEditable]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           ('AcademicYear',
		   'The academic year of the allocations that should be shown, e.g. 201819.',
		   0,
		   1,
		   @currentDateTime,
		   @currentDateTime,
		   @migrationUser)
END

if NOT EXISTS (SELECT Id FROM Settings WHERE SettingName = 'AcademyAndSchoolAcademicYear')
BEGIN
		INSERT INTO [Settings]
           ([SettingName]
           ,[SettingDescription]
           ,[ValueDataType]
           ,[ValuesAreEditable]
           ,[CreatedAt]
           ,[LastUpdatedAt]
           ,[LastUpdatedBy])
     VALUES
           ('AcademyAndSchoolAcademicYear',
		   'The academy academic year of the allocations that should be shown, e.g. 201819.',
		   0,
		   1,
		   @currentDateTime,
		   @currentDateTime,
		   @migrationUser)
END

SELECT @academicYearSettingId =  Id FROM Settings WHERE SettingName = 'AcademicYear'  
SELECT @academyAndSchoolAcademicYearSettingId =  Id FROM Settings WHERE SettingName = 'AcademyAndSchoolAcademicYear' 

-- DELETE  NMSS AcademyAndSchoolAcademicYear setting if it exists
IF EXISTS(SELECT 1 FROM [dbo].[SettingValues] WHERE FundingStreamId = @fundingStreamId AND SettingId = @academyAndSchoolAcademicYearSettingId)
BEGIN
DELETE FROM [dbo].[SettingValues]
WHERE   FundingStreamId = @fundingStreamId AND SettingId = @academyAndSchoolAcademicYearSettingId
END

-- INSERT  NMSS AcademicYear setting 

IF NOT EXISTS(SELECT 1 FROM [dbo].[SettingValues] WHERE FundingStreamId = @fundingStreamId AND SettingId = @academicYearSettingId)
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
           ,@academicYearSettingId
           ,'202122'
           ,@currentDateTime
           ,@currentDateTime
           ,@migrationUser)
END

COMMIT TRAN T1;