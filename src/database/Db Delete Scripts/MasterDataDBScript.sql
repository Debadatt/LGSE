

/*[Roles]*/
--Insert master Data in [Roles].
--Abhijeet 
--26-09-2018
INSERT [dbo].[Roles] ([Id], [RoleName], [Description], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'14ed68ab-e5ac-41d3-aa71-25721208be15', N'Incident Controller', N'Incident Controller', N'Incident Controller', N'admin@lgse.com', CAST(N'2018-09-02T18:58:15.0233447+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Roles] ([Id], [RoleName], [Description], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'41D9E012-8D5C-49B5-89A9-7FB9386D9590', N'Isolator', N'Isolator
', N'sneha', N'NUrLL', CAST(N'2018-08-23T11:39:40.3949392+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Roles] ([Id], [RoleName], [Description], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'424008B4-3688-4FD9-A110-FDDBD883AFDD', N'Support Staff ', N'Support Staff ', N'Abhi', NULL, CAST(N'2018-09-12T09:59:02.5490865+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Roles] ([Id], [RoleName], [Description], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'454B87A7-C15F-43D8-93F8-CE376F5F7849', N'Cell Manager', N'Cell Manager', N'Abhi', NULL, CAST(N'2018-09-12T09:58:41.9864831+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Roles] ([Id], [RoleName], [Description], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'8FE7DBCB-DCC3-4AC1-803A-5336621C8359', N'Engineer', N'Engineer', N'sneha', NULL, CAST(N'2018-08-23T11:38:58.8165663+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Roles] ([Id], [RoleName], [Description], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'9FA0DA6D-5A60-42B9-BE57-40BBDF8CD4BD', N'Zone Controller', N'Zone Controller', N'Abhi', NULL, CAST(N'2018-09-12T09:57:51.2517339+00:00' AS DateTimeOffset), NULL, 0)
GO



/*[PropertyStatusMstrs]*/
--Insert master Data in PropertyStatusMstrs.
--Abhijeet 
--26-09-2018
INSERT [dbo].[PropertyStatusMstrs] ([Id], [Status], [DisplayOrder],[ShortText], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'29891F61-D32D-496E-AB1E-F88FBF64807E', N'Restored', 3,'RS', N'Rajesh', NULL, CAST(N'2018-08-29T07:52:15.9985177+00:00' AS DateTimeOffset), CAST(N'2018-08-29T07:52:15.9985177+00:00' AS DateTimeOffset), 0)
GO
INSERT [dbo].[PropertyStatusMstrs] ([Id], [Status], [DisplayOrder],[ShortText], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'44899bde-4263-46e6-9a29-3d1153c0d02c', N'Isolated', 2,'IS', N'Rajesh', N'', CAST(N'2018-08-29T07:51:02.4857189+00:00' AS DateTimeOffset), CAST(N'2018-08-29T07:52:15.9985177+00:00' AS DateTimeOffset), 0)
GO
INSERT [dbo].[PropertyStatusMstrs] ([Id], [Status], [DisplayOrder],[ShortText], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'59E7FAC8-4A83-4D80-9603-B72B82C1AA35', N'No Access', 0,'NA', N'Abhi', NULL, CAST(N'2018-09-07T09:03:44.8102276+00:00' AS DateTimeOffset), CAST(N'2018-08-29T07:52:15.9985177+00:00' AS DateTimeOffset), 0)
GO
INSERT [dbo].[PropertyStatusMstrs] ([Id], [Status], [DisplayOrder],[ShortText], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'EA188FEE-714C-49FA-AA70-A85318FAFC41', N'No Gas Connection', 1,'NC', N'Abhi', NULL, CAST(N'2018-09-07T09:03:17.0600501+00:00' AS DateTimeOffset), CAST(N'2018-08-29T07:52:15.9985177+00:00' AS DateTimeOffset), 0)
GO
INSERT [dbo].[PropertyStatusMstrs] ([Id], [Status], [DisplayOrder], [ShortText], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'7A279257-8CFA-488C-A807-FDBD05A81DD0', N'', 4, N'', N'Abhi', NULL, CAST(N'2018-10-21T08:19:22.2764095+00:00' AS DateTimeOffset), NULL, 1)
GO

/*[PropertySubStatusMstrs]*/
--Insert master Data in [PropertySubStatusMstrs].
--Abhijeet 
--26-09-2018
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'4809AC3E-1A86-4A97-868F-57ED1AC1A69E', N'MeterBox', N'44899bde-4263-46e6-9a29-3d1153c0d02c', 2, N'Rajesh', NULL, CAST(N'2018-08-29T07:56:56.6179851+00:00' AS DateTimeOffset), CAST(N'2018-08-29T07:56:56.6179851+00:00' AS DateTimeOffset), 0)
GO
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'68DB2CC9-9DCD-4853-B6E7-BF8F952442F2', N'LT', N'44899bde-4263-46e6-9a29-3d1153c0d02c', 3, N'Rajesh', NULL, CAST(N'2018-08-29T07:57:09.8473817+00:00' AS DateTimeOffset), CAST(N'2018-08-29T07:56:56.6179851+00:00' AS DateTimeOffset), 0)
GO
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'930762E3-3D0F-4BAB-9F5B-632B87121F17', N'ECV', N'44899bde-4263-46e6-9a29-3d1153c0d02c', 1, N'Rajesh', NULL, CAST(N'2018-08-29T07:56:47.4072017+00:00' AS DateTimeOffset), CAST(N'2018-08-29T07:56:56.6179851+00:00' AS DateTimeOffset), 0)
GO
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'93730CA0-CB67-4FCE-B4CD-668C100C9A40', N'Gas Off At ECV', N'59E7FAC8-4A83-4D80-9603-B72B82C1AA35', 0, N'Abhi', NULL, CAST(N'2018-09-28T09:04:43.3191871+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'B80BE8B2-A3E0-4B4A-B893-24AEBA30C729', N'ECV Alternative Heating/ Cooking Left', N'44899bde-4263-46e6-9a29-3d1153c0d02c', 4, N'Rajesh', NULL, CAST(N'2018-08-29T07:58:00.6469293+00:00' AS DateTimeOffset), CAST(N'2018-08-29T07:56:56.6179851+00:00' AS DateTimeOffset), 0)
GO
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'CFF66403-7D1B-403F-A63B-6A8F8283BA0B', N'Gas Off At MeterBox', N'59E7FAC8-4A83-4D80-9603-B72B82C1AA35', 2, N'Abhi', NULL, CAST(N'2018-09-28T09:05:53.6945723+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'D387D75C-1E59-4011-A740-FC3CFE9EFA17', N'Gas Off At LT', N'59E7FAC8-4A83-4D80-9603-B72B82C1AA35', 1, N'Abhi', NULL, CAST(N'2018-09-28T09:05:10.3349875+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'E34AE88E-3B4A-474A-BE61-99D7AF316AC2', N'Self', N'44899bde-4263-46e6-9a29-3d1153c0d02c', 0, N'Rajesh', NULL, CAST(N'2018-08-29T07:56:16.5099289+00:00' AS DateTimeOffset), CAST(N'2018-08-29T07:56:56.6179851+00:00' AS DateTimeOffset), 0)
GO
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'C1FE24FC-F4D1-4681-97DF-5A7C214444A3', N'Self Alternative Heating / Cooking Left', N'44899bde-4263-46e6-9a29-3d1153c0d02c', 7, N'Abhi', NULL, CAST(N'2018-10-21T08:45:20.1869078+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'E3B717D1-3CA8-4F78-9206-2B093319B34F', N'LT Alternative Heating / Cooking Left', N'44899bde-4263-46e6-9a29-3d1153c0d02c', 5, N'Abhi', NULL, CAST(N'2018-10-21T08:43:42.7177425+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[PropertySubStatusMstrs] ([Id], [SubStatus], [PropertyStatusMstrsId], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'F93DE654-7AD1-408B-82F9-5FDE3206BF7A', N'Meterbox Alternative Heating / Cooking Left', N'44899bde-4263-46e6-9a29-3d1153c0d02c', 6, N'Abhi', NULL, CAST(N'2018-10-21T08:44:50.4682014+00:00' AS DateTimeOffset), NULL, 0)
GO

/*[RoleStatusMaps]*/
--Insert master Data in [RoleStatusMaps].
--Abhijeet 
--26-09-2018

INSERT [dbo].[RoleStatusMaps] ([id], [RoleId], [StatusId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'41C2EB3F-4EBE-4D1E-AE8B-B4EDDC610B4B', N'8FE7DBCB-DCC3-4AC1-803A-5336621C8359', N'29891F61-D32D-496E-AB1E-F88FBF64807E', N'Rajesh', NULL, CAST(N'2018-09-19T03:53:15.4250512+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RoleStatusMaps] ([id], [RoleId], [StatusId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'526A128A-E00B-443B-ABDB-75CAB31889B0', N'41D9E012-8D5C-49B5-89A9-7FB9386D9590', N'EA188FEE-714C-49FA-AA70-A85318FAFC41', N'Rajesh', NULL, CAST(N'2018-09-19T03:54:30.2693386+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RoleStatusMaps] ([id], [RoleId], [StatusId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'679CB09E-DA0A-40FB-BC1E-A60A1E8505F7', N'8FE7DBCB-DCC3-4AC1-803A-5336621C8359', N'EA188FEE-714C-49FA-AA70-A85318FAFC41', N'Rajesh', NULL, CAST(N'2018-09-19T03:53:53.5815488+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RoleStatusMaps] ([id], [RoleId], [StatusId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'AB590CB3-F587-40E3-BAAC-AF08E6582A3A', N'8FE7DBCB-DCC3-4AC1-803A-5336621C8359', N'59E7FAC8-4A83-4D80-9603-B72B82C1AA35', N'Rajesh', NULL, CAST(N'2018-09-19T03:53:35.0502611+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RoleStatusMaps] ([id], [RoleId], [StatusId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'D46AF173-1150-441F-99A0-23AFBC6A81D6', N'41D9E012-8D5C-49B5-89A9-7FB9386D9590', N'44899bde-4263-46e6-9a29-3d1153c0d02c', N'Rajesh', NULL, CAST(N'2018-09-19T03:54:45.9725478+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RoleStatusMaps] ([id], [RoleId], [StatusId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'E4BF7AFE-13E0-48AB-89D7-CB90532FCF41', N'41D9E012-8D5C-49B5-89A9-7FB9386D9590', N'59E7FAC8-4A83-4D80-9603-B72B82C1AA35', N'Rajesh', NULL, CAST(N'2018-09-19T03:54:54.7226216+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RoleStatusMaps] ([id], [RoleId], [StatusId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'F0D01D16-C1F4-4CFE-96AC-8EB7D4EE2D03', N'8FE7DBCB-DCC3-4AC1-803A-5336621C8359', N'44899bde-4263-46e6-9a29-3d1153c0d02c', N'Rajesh', NULL, CAST(N'2018-09-19T03:53:26.0970438+00:00' AS DateTimeOffset), NULL, 0)
GO

/*[Domains].*/
-- Insert master Data in [Domains].
--Abhijeet 
--26-09-2018
INSERT [dbo].[Domains] ([Id], [OrgName], [DomainName], [IsActive], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'30E05898-9BB2-4EB0-88DD-95036CB106C2', N'Sagacity', N'sagacitysoftware.co.in', 1, N'sneha', NULL, CAST(N'2018-08-22T13:53:51.6571344+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Domains] ([Id], [OrgName], [DomainName], [IsActive], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'73B6C6A1-5E7B-4CB7-A328-1434EBDBC3F5', N'LGSE', N'lgse.com', 1, N'Abhi', NULL, CAST(N'2018-09-12T10:19:52.0732742+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Domains] ([Id], [OrgName], [DomainName], [IsActive], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'B6C0629B-4F37-4F13-B166-1813CB705833', N'TestGmail', N'gmail.com', 1, N'sneha', NULL, CAST(N'2018-08-22T13:52:32.2816356+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Domains] ([Id], [OrgName], [DomainName], [IsActive], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'BF827662-01F2-414E-9528-E7380B031F21', N'Wales & West Utilities', N'wwutilities.co.uk', 1, N'sneha', NULL, CAST(N'2018-08-22T13:53:28.8601180+00:00' AS DateTimeOffset), NULL, 0)
GO



/*[CategoriesMstrs]*/
--Insert master Data in [CategoriesMstrs].
--Abhijeet 
--26-09-2018
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'0345FCC8-151C-43AB-A5B4-7846BCF3594B', N'Minor - 3rd Party Damage', N'Minor - 3rd Party Damage', 1, N'Pooja', NULL, CAST(N'2018-09-26T09:54:33.1886854+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'08F5BB83-B70A-4FD3-8B80-F5E61F019A43', N'Minor - Network Condition', N'Minor - Network Condition', 3, N'Pooja', NULL, CAST(N'2018-09-26T09:56:51.4231010+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'0AF03FBB-F24C-4CAA-974B-D91C004BE4F9', N'Severe - GDN Initiated Activities', N'Severe - GDN Initiated Activities', 14, N'Pooja', NULL, CAST(N'2018-09-26T10:07:50.9578767+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'1855AE27-5B30-41B1-A61F-D78E654C36CC', N'Severe - Network Condition', N'Severe - Network Condition', 15, N'Pooja', NULL, CAST(N'2018-09-26T10:15:28.9427704+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'1BAD8850-1A39-4077-A886-4FE1C7170440', N'Large', N'Large', 4, N'sneha', NULL, CAST(N'2018-08-30T09:53:29.5265157+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'2903B1B3-9E9F-4D47-926D-B7A2FD616B30', N'Large - 3rd Party Damage', N'Large - 3rd Party Damage', 5, N'Pooja', NULL, CAST(N'2018-09-26T09:58:00.5949166+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'2C462711-FE65-4BF8-B57D-8A4D34FD28CF', N'Minor - GDN Initiated Activities', N'Minor - GDN Initiated Activities', 2, N'Pooja', NULL, CAST(N'2018-09-26T09:55:35.8761877+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'3FA3D2A1-B2C8-4C31-932F-0698D75F8F62', N'Large - Network Condition', N'Large - Network Condition', 7, N'Pooja', NULL, CAST(N'2018-09-26T10:00:28.0802220+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'426E0308-62EA-429E-83CA-F627E8CB4319', N'Major - 3rd Party Damage', N'Major - 3rd Party Damage', 9, N'Pooja', NULL, CAST(N'2018-09-26T10:01:28.3304206+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'724221CB-A13C-4522-8907-67B56EE82A18', N'Large - GDN Initiated Activities', N'Large - GDN Initiated Activities', 6, N'Pooja', NULL, CAST(N'2018-09-26T09:59:29.0327534+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'74098426-837D-4BF2-AEE9-13B59F587571', N'Major - GDN Initiated Activities', N'Major - GDN Initiated Activities', 10, N'Pooja', NULL, CAST(N'2018-09-26T10:02:17.8307225+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'7A56848C-C649-4590-8AC2-839F42F225C6', N'Major - Network Condition', N'Major - Network Condition', 11, N'Pooja', NULL, CAST(N'2018-09-26T10:03:08.2060704+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'7F7761FD-2D27-4060-8438-D470DBAB2673', N'Severe', N'Severe', 12, N'Pooja', NULL, CAST(N'2018-09-26T10:04:12.0970743+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'8372787D-9B64-44FB-8520-D69F8381DF1C', N'Severe - 3rd Party Damage', N'Severe - 3rd Party Damage', 13, N'Pooja', NULL, CAST(N'2018-09-26T10:04:55.4098783+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[CategoriesMstrs] ([Id], [Category], [Description], [DisplayOrder], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'887C559C-6EAF-4398-9E59-63DEDF61EC17', N'Minor', N'Minor', 0, N'sneha', NULL, CAST(N'2018-08-30T09:52:39.4949504+00:00' AS DateTimeOffset), NULL, 0)
GO

/*[Features]*/
--Insert master Data in [Features].
--Abhijeet 
--26-09-2018
INSERT [dbo].[Features] ([Id], [FeatureName], [FeatureText], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'36506bf6-b216-41d6-ae01-cfda11b78291
', N'PORTALMANAGEMENT', N'Portal Management', N'rajesh.kotaprolu@gmail.com', NULL, CAST(N'2018-09-24T20:14:16.0965365+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Features] ([Id], [FeatureName], [FeatureText], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'4e6ec67f-74e3-4cd3-ae85-ebfae9bb9f0a
', N'RESOURCEMGT', N'Resource Management', N'rajesh.kotaprolu@gmail.com', NULL, CAST(N'2018-09-24T20:10:48.0072500+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Features] ([Id], [FeatureName], [FeatureText], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'8186cdd3-52ae-40d2-afe8-51bbba2e20bc
', N'INCIDENTMGT', N'Incident Management', N'rajesh.kotaprolu@gmail.com', NULL, CAST(N'2018-09-24T20:09:54.7733567+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Features] ([Id], [FeatureName], [FeatureText], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'e525ffe7-5166-4812-acec-c2ecc88c6dc5', N'DASHBOARD', N'Dashboard', N'rajesh.kotaprolu@gmail.com', NULL, CAST(N'2018-09-24T20:13:39.0195574+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[Features] ([Id], [FeatureName], [FeatureText], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'2df4c148-aaf1-417a-85f9-31408fe5b4e4', N'ASSIGNEDMPRN', N'Assigned MPRN', N'Abhi', NULL, CAST(N'2018-09-24T20:13:39.0195574+00:00' AS DateTimeOffset), NULL, 0)
GO


/*[RolePermissions]*/
--Insert master Data in [RolePermissions].
--Abhijeet 
--26-09-2018
--INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'2df4c148-aaf1-417a-85f9-31408fe5b4e4', N'N', N'A', N'N', N'N', N'45db9ead-5260-4263-aaff-32671f5d85b3', N'36506bf6-b216-41d6-ae01-cfda11b78291
--', N'admin@lgse.com', NULL, CAST(N'2018-09-28T15:44:51.2066757+00:00' AS DateTimeOffset), NULL, 0)
--GO
--INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'04903e77-d2f1-4ae8-bb8e-a64918f40878', N'N', N'A', N'N', N'N', N'45db9ead-5260-4263-aaff-32671f5d85b3', N'4e6ec67f-74e3-4cd3-ae85-ebfae9bb9f0a
--', N'admin@lgse.com', NULL, CAST(N'2018-09-28T15:44:51.2223262+00:00' AS DateTimeOffset), NULL, 0)
--GO
--INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'b1d857cf-9d31-4d71-a02b-f8aad12d4a1c', N'N', N'A', N'N', N'N', N'45db9ead-5260-4263-aaff-32671f5d85b3', N'8186cdd3-52ae-40d2-afe8-51bbba2e20bc
--', N'admin@lgse.com', NULL, CAST(N'2018-09-28T15:44:51.2223262+00:00' AS DateTimeOffset), NULL, 0)
--GO
--INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'bcdb8534-89bc-47c7-b2d0-9de2734ffd9b', N'N', N'A', N'N', N'N', N'45db9ead-5260-4263-aaff-32671f5d85b3', N'e525ffe7-5166-4812-acec-c2ecc88c6dc5', N'admin@lgse.com', NULL, CAST(N'2018-09-28T15:44:51.2379217+00:00' AS DateTimeOffset), NULL, 0)
--GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'52655916-b915-4428-926e-21e2e367ef9c', N'N', N'N', N'N', N'N', N'8FE7DBCB-DCC3-4AC1-803A-5336621C8359', N'36506bf6-b216-41d6-ae01-cfda11b78291
', N'admin@lgse.com', N'admin@lgse.com', CAST(N'2018-09-28T16:02:23.1637894+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'9f6c5925-8a16-41d0-bbd1-f4fea94bf9b6', N'N', N'N', N'N', N'N', N'8FE7DBCB-DCC3-4AC1-803A-5336621C8359', N'4e6ec67f-74e3-4cd3-ae85-ebfae9bb9f0a
', N'admin@lgse.com', N'admin@lgse.com', CAST(N'2018-09-28T16:02:23.1637894+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'e71ed933-edb4-43ed-9ebb-4ac08dacd22b', N'N', N'N', N'N', N'N', N'8FE7DBCB-DCC3-4AC1-803A-5336621C8359', N'8186cdd3-52ae-40d2-afe8-51bbba2e20bc
', N'admin@lgse.com', N'admin@lgse.com', CAST(N'2018-09-28T16:02:23.1794079+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'db1a1804-16c0-40a6-9855-486ecc7c10a9', N'A', N'A', N'A', N'A', N'14ed68ab-e5ac-41d3-aa71-25721208be15', N'e525ffe7-5166-4812-acec-c2ecc88c6dc5', N'admin@lgse.com', N'admin@lgse.com', CAST(N'2018-09-27T18:43:17.1046661+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'f0b54e89-ae8a-4e66-9eff-73d6ffa4fae3', N'A', N'A', N'A', N'A', N'14ed68ab-e5ac-41d3-aa71-25721208be15', N'4e6ec67f-74e3-4cd3-ae85-ebfae9bb9f0a
', N'admin@lgse.com', N'admin@lgse.com', CAST(N'2018-09-28T05:35:46.2171721+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'5fc396ca-ec90-433e-9e1e-5671ca048e3c', N'A', N'N', N'N', N'N', N'9FA0DA6D-5A60-42B9-BE57-40BBDF8CD4BD', N'e525ffe7-5166-4812-acec-c2ecc88c6dc5', N'admin@lgse.com', NULL, CAST(N'2018-09-28T11:18:16.0789668+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'7AC04A25-4CF6-4E0C-8469-7DD5D8F60611', N'A', N'A', N'A', N'A', N'41D9E012-8D5C-49B5-89A9-7FB9386D9590', N'36506bf6-b216-41d6-ae01-cfda11b78291
', N'Rajesh', N'admin@lgse.com', CAST(N'2018-09-26T18:31:04.1088018+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'8186cdd3-52ae-40d2-afe8-51bbba2e20bc
', N'A', N'A', N'A', N'A', N'14ed68ab-e5ac-41d3-aa71-25721208be15', N'8186cdd3-52ae-40d2-afe8-51bbba2e20bc
', N'Abhi', N'admin@lgse.com', CAST(N'2018-09-27T08:49:04.9291141+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'e9b8fbaa-0a1f-4803-b86b-8cd120c829ae', N'N', N'A', N'A', N'N', N'41D9E012-8D5C-49B5-89A9-7FB9386D9590', N'4e6ec67f-74e3-4cd3-ae85-ebfae9bb9f0a
', N'admin@lgse.com', N'admin@lgse.com', CAST(N'2018-09-27T14:17:05.8504331+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'616b5996-8482-4612-a556-99c5b7dc2ec2', N'A', N'A', N'A', N'A', N'14ed68ab-e5ac-41d3-aa71-25721208be15', N'36506bf6-b216-41d6-ae01-cfda11b78291
', N'admin@lgse.com', N'admin@lgse.com', CAST(N'2018-09-28T06:31:04.3535787+00:00' AS DateTimeOffset), NULL, 0)
GO
INSERT [dbo].[RolePermissions] ([Id], [CreatePermission], [ReadPermission], [UpdatePermission], [DeletePermission], [RoleId], [FeatureId], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'5bb67954-09ea-46d4-9ec0-62a508259485', N'A', N'A', N'N', N'N', N'9FA0DA6D-5A60-42B9-BE57-40BBDF8CD4BD', N'4e6ec67f-74e3-4cd3-ae85-ebfae9bb9f0a
', N'admin@lgse.com', N'admin@lgse.com', CAST(N'2018-09-28T06:52:40.5918607+00:00' AS DateTimeOffset), NULL, 0)
GO

/*[IncidentOverviewMstrs]*/
--Insert master Data in [IncidentOverviewMstrs].
--Abhijeet 
--26-09-2018
 INSERT [dbo].[IncidentOverviewMstrs] ([Id], [Description], [DefaultText], [IsActive], [CreatedBy], [ModifiedBy], [CreatedAt], [UpdatedAt], [Deleted]) VALUES (N'E5D1C5AE-6CD1-4961-8692-3521433383F6', N'', N'This is the WWU LGSE application used to manage incidents requiring isolation and restoration activities on the WWU gas network. The application is for authorised users only. All activity is logged and regularly checked. By using this application, you are confirming that you are authorised to undertake the activities that are assigned to you and use of this system constitutes consent to these terms.', 1, N'Abhi', NULL, CAST(N'2018-10-11T12:44:31.6820470+00:00' AS DateTimeOffset), NULL, 0)
GO