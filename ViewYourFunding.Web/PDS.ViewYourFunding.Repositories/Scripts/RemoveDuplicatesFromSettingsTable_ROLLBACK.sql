BEGIN TRAN T1;
DECLARE
	@currentDateTime DATETIME,
    @migrationUser nvarchar(128) = 'Migration'

SELECT	@currentDateTime = GETDATE()

DELETE FROM [dbo].[SettingValues] 
GO

DELETE FROM [dbo].[Settings] 
GO

SET IDENTITY_INSERT [dbo].[Settings] ON 

GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (1, N'AcademicYear', N'The academic year of the allocations that should be shown, e.g. 201819.', 1, 1, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (2, N'FinancialYear', N'The financial year of the allocations that should be shown, e.g. 201819.', 1, 1, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (3, N'ProviderDownloadSizeInBytes', N'The download size (in bytes) for the provider spreadsheet.', 1, 1, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (4, N'ProviderDownloadSizeInBytes', N'The download size (in bytes) for the provider spreadsheet.', 1, 1, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (5, N'OrganisationDownloadSizeInBytes', N'The download size (in bytes) for the organisation spreadsheet.', 1, 1, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (6, N'OrganisationDownloadSizeInBytes', N'The download size (in bytes) for the organisation spreadsheet.', 1, 1, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (7, N'NextPaymentDateTypeCode', N'The next payment date type code.', 0, 1, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (8, N'HistoricAllocationsAreExternalYear', N'The year histroic allocations are just external links.', 1, 1, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (9, N'LAGroupingReason', N'The grouping reason to use for LAs.', 0, 0, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (10, N'FundingDocumentFileType', N'The funding document type', 0, 1, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (11, N'ProviderDownloadSizeInBytes', N'The download size (in bytes) for the provider spreadsheet.', 1, 1, @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[Settings] ([Id], [SettingName], [SettingDescription], [ValueDataType], [ValuesAreEditable], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (12, N'UpdateSpreadsheetUrl', N'The url to request to trigger this funding stream''s spreadsheet(s) to be generated.', 0, 0, @currentDateTime, @currentDateTime, @migrationUser)
GO
SET IDENTITY_INSERT [dbo].[Settings] OFF
GO
SET IDENTITY_INSERT [dbo].[SettingValues] ON 

GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (1, 1, 1, N'201920', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (2, 2, 2, N'202021', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (3, 3, 1, N'202122', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (4, 1, 12, N'/single-funding-statement/latest/api/funding/GenerateFundingDocument?fundingStreamCode={0}&fundingPeriodCode={1}&cutoffDate={2}&publicationDate={3}&modelVersion={4}&waitForIndexBuild=true', @currentDateTime, @currentDateTime, @migrationUser)
GO																																																																																			  					
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (5, 3, 12, N'/single-funding-statement/latest/api/funding/GenerateFundingDocument?fundingStreamCode={0}&fundingPeriodCode={1}&cutoffDate={2}&publicationDate={3}&modelVersion={4}&waitForIndexBuild=true', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (6, 3, 10, N'csv', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (7, 2, 9, N'Payment', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (8, 2, 8, N'2019', @currentDateTime, @currentDateTime , @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (9, 2, 3, N'5000', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (10, 1, 6, N'7000', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (11, 2, 5, N'28000', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (12, 1, 4, N'5000', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (13, 3, 11, N'206000', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (14, 1, 7, N'MS', @currentDateTime, @currentDateTime, @migrationUser)
GO
INSERT [dbo].[SettingValues] ([Id], [FundingStreamId], [SettingId], [Value], [CreatedAt], [LastUpdatedAt], [LastUpdatedBy]) VALUES (15, 2, 12, N'/single-funding-statement/latest/api/funding/GenerateFundingDocument?fundingStreamCode={0}&fundingPeriodCode={1}&cutoffDate={2}&publicationDate={3}&modelVersion={4}&waitForIndexBuild=true', @currentDateTime, @currentDateTime, @migrationUser)
GO
SET IDENTITY_INSERT [dbo].[SettingValues] OFF
GO


COMMIT TRAN T1;