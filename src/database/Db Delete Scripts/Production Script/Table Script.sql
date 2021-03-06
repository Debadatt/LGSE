USE [SQLDB-LGSEPROD]
GO
/****** Object:  Table [dbo].[AuditTrials]    Script Date: 11/27/2018 12:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditTrials](
	[Id] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NULL,
	[DeviceId] [nvarchar](128) NULL,
	[TypeofOperation] [nvarchar](128) NULL,
	[Status] [nvarchar](32) NULL,
	[TokenId] [nvarchar](max) NULL,
	[OperationTimeStamp] [datetimeoffset](7) NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[ModifiedBy] [nvarchar](255) NULL,
	[Version] [timestamp] NOT NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[IPAddress] [nvarchar](32) NULL,
 CONSTRAINT [PK_dbo.AuditTrial] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoriesMstrs]    Script Date: 11/27/2018 12:12:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriesMstrs](
	[Id] [nvarchar](128) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[DisplayOrder] [int] NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_IncidentCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_CategoriesMstrs] UNIQUE NONCLUSTERED 
(
	[Category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Domains]    Script Date: 11/27/2018 12:12:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Domains](
	[Id] [nvarchar](128) NOT NULL,
	[OrgName] [nvarchar](250) NOT NULL,
	[DomainName] [nvarchar](250) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_Domains] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Domains] UNIQUE NONCLUSTERED 
(
	[DomainName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Features]    Script Date: 11/27/2018 12:12:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Features](
	[Id] [nvarchar](128) NOT NULL,
	[FeatureName] [nvarchar](250) NOT NULL,
	[FeatureText] [nvarchar](250) NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_Features] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Features] UNIQUE NONCLUSTERED 
(
	[FeatureName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidentHistories]    Script Date: 11/27/2018 12:12:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidentHistories](
	[Id] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[Incidentid] [nvarchar](128) NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_IncidentHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidentOverviewMstrs]    Script Date: 11/27/2018 12:12:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidentOverviewMstrs](
	[Id] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DefaultText] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](250) NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NULL,
	[Version] [timestamp] NULL,
 CONSTRAINT [PK_IncidentOverviewMstr] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidentPropsStatusCounts]    Script Date: 11/27/2018 12:12:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidentPropsStatusCounts](
	[Id] [nvarchar](128) NOT NULL,
	[IncidentId] [nvarchar](128) NULL,
	[NS] [int] NULL,
	[NA] [int] NULL,
	[NC] [int] NULL,
	[IS] [int] NULL,
	[RS] [int] NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_IncidentPropsStatusCounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Incidents]    Script Date: 11/27/2018 12:12:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Incidents](
	[Id] [nvarchar](128) NOT NULL,
	[IncidentId] [nvarchar](50) NOT NULL,
	[CategoriesMstrId] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[Notes] [nvarchar](2000) NULL,
	[NoOfPropsAffected] [int] NULL,
	[NoOfZones] [int] NULL,
	[NoOfCells] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Status] [int] NULL,
	[ClosingNotes] [nvarchar](2000) NULL,
	[NoOfPropsIsolated] [int] NULL,
	[NoOfPropsRestored] [int] NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_Incidents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Incidents] UNIQUE NONCLUSTERED 
(
	[IncidentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Properties]    Script Date: 11/27/2018 12:12:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Properties](
	[Id] [nvarchar](128) NOT NULL,
	[MPRN] [nvarchar](50) NOT NULL,
	[BuildingName] [nvarchar](250) NULL,
	[SubBuildingName] [nvarchar](250) NULL,
	[MCBuildingName] [nvarchar](250) NULL,
	[MCSubBuildingName] [nvarchar](250) NULL,
	[BuildingNumber] [nvarchar](50) NULL,
	[PrincipalStreet] [nvarchar](250) NULL,
	[DependentStreet] [nvarchar](250) NULL,
	[PostTown] [nvarchar](250) NULL,
	[LocalityName] [nvarchar](250) NULL,
	[DependentLocality] [nvarchar](250) NULL,
	[Country] [nvarchar](50) NULL,
	[Postcode] [nvarchar](50) NULL,
	[PriorityCustomer] [bit] NOT NULL,
	[Zone] [nvarchar](250) NULL,
	[Cell] [nvarchar](250) NULL,
	[IncidentId] [nvarchar](128) NULL,
	[CellManagerId] [nvarchar](128) NULL,
	[ZoneManagerId] [nvarchar](128) NULL,
	[StatusId] [nvarchar](128) NULL,
	[SubStatusId] [nvarchar](128) NULL,
	[Status] [int] NULL,
	[Latitude] [nvarchar](250) NULL,
	[Longitude] [nvarchar](250) NULL,
	[IsIsolated] [bit] NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_Properties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyStatusMstrs]    Script Date: 11/27/2018 12:12:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyStatusMstrs](
	[Id] [nvarchar](128) NOT NULL,
	[Status] [nvarchar](250) NULL,
	[DisplayOrder] [int] NOT NULL,
	[ShortText] [nvarchar](100) NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_StatusMstr] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_PropertyStatusMstrs] UNIQUE NONCLUSTERED 
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertySubStatusMstrs]    Script Date: 11/27/2018 12:12:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertySubStatusMstrs](
	[Id] [nvarchar](128) NOT NULL,
	[SubStatus] [nvarchar](250) NOT NULL,
	[PropertyStatusMstrsId] [nvarchar](128) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_PropertySubStatusMstrs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_PropertySubStatusMstrs] UNIQUE NONCLUSTERED 
(
	[PropertyStatusMstrsId] ASC,
	[SubStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyUserMaps]    Script Date: 11/27/2018 12:12:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyUserMaps](
	[Id] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[PropertyId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_PropertyUserMap_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyUserNotes]    Script Date: 11/27/2018 12:12:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyUserNotes](
	[Id] [nvarchar](128) NOT NULL,
	[Notes] [nvarchar](2000) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[PropertyId] [nvarchar](128) NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_PropertyNotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyUserStatus]    Script Date: 11/27/2018 12:12:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyUserStatus](
	[id] [nvarchar](128) NOT NULL,
	[PropertyId] [nvarchar](128) NOT NULL,
	[StatusId] [nvarchar](128) NULL,
	[PropertySubStatusMstrsId] [nvarchar](128) NULL,
	[StatusChangedOn] [datetimeoffset](7) NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[IncidentId] [nvarchar](128) NULL,
	[PropertyUserMapsId] [nvarchar](128) NULL,
	[Notes] [nvarchar](2000) NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_PropertyStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolePermissions]    Script Date: 11/27/2018 12:12:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePermissions](
	[Id] [nvarchar](128) NOT NULL,
	[CreatePermission] [char](1) NOT NULL,
	[ReadPermission] [char](1) NOT NULL,
	[UpdatePermission] [char](1) NOT NULL,
	[DeletePermission] [char](1) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[FeatureId] [nvarchar](128) NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_RolePermissions] UNIQUE NONCLUSTERED 
(
	[FeatureId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/27/2018 12:12:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [nvarchar](128) NOT NULL,
	[RoleName] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Roles] UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleStatusMaps]    Script Date: 11/27/2018 12:12:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleStatusMaps](
	[id] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[StatusId] [nvarchar](128) NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_RoleStatusMap] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_RoleStatusMaps] UNIQUE NONCLUSTERED 
(
	[RoleId] ASC,
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoleMaps]    Script Date: 11/27/2018 12:12:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleMaps](
	[Id] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
	[IsPreferredRole] [bit] NULL,
 CONSTRAINT [PK_UserRoleMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/27/2018 12:12:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[FirstName] [nvarchar](250) NOT NULL,
	[LastName] [nvarchar](250) NOT NULL,
	[DomainId] [nvarchar](128) NOT NULL,
	[EmployeeId] [nvarchar](50) NULL,
	[EUSR] [nvarchar](250) NULL,
	[ContactNo] [nvarchar](15) NULL,
	[IsAvailable] [bit] NOT NULL,
	[IsActivated] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[Password] [varbinary](max) NULL,
	[Salt] [varbinary](max) NULL,
	[IsTermsAccepted] [bit] NOT NULL,
	[PwdStartDate] [datetimeoffset](7) NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
	[OTPGeneratedAt] [datetimeoffset](7) NULL,
	[OTPCode] [nvarchar](32) NULL,
	[IsActiveUser] [bit] NOT NULL,
	[IsLoggedIn] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Users] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditTrials] ADD  CONSTRAINT [DF__AuditTrials__Id__00AA174D]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[AuditTrials] ADD  CONSTRAINT [DF__AuditTria__Creat__019E3B86]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[CategoriesMstrs] ADD  CONSTRAINT [DF_CategoriesMstr_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[CategoriesMstrs] ADD  CONSTRAINT [DF_CategoriesMstrs_DisplayOrder]  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[CategoriesMstrs] ADD  CONSTRAINT [DF_IncidentCategories_Created]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[CategoriesMstrs] ADD  CONSTRAINT [DF_CategoriesMstr_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Features] ADD  CONSTRAINT [DF_Features_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Features] ADD  CONSTRAINT [DF_Features_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[IncidentHistories] ADD  CONSTRAINT [DF_IncidentHistory_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[IncidentHistories] ADD  CONSTRAINT [DF_IncidentHistory_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[IncidentHistories] ADD  CONSTRAINT [DF_IncidentHistory_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[IncidentOverviewMstrs] ADD  CONSTRAINT [DF_IncidentOverviewMstr_DefaultActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[IncidentOverviewMstrs] ADD  CONSTRAINT [DF_IncidentOverviewMstr_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[IncidentOverviewMstrs] ADD  CONSTRAINT [DF_IncidentOverviewMstr_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[IncidentPropsStatusCounts] ADD  CONSTRAINT [DF_IncidentPropsStatusCounts_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[IncidentPropsStatusCounts] ADD  CONSTRAINT [DF_IncidentPropsStatusCounts_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[IncidentPropsStatusCounts] ADD  CONSTRAINT [DF_IncidentPropsStatusCounts_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[IncidentPropsStatusCounts] ADD  CONSTRAINT [DF_IncidentPropsStatusCounts_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Table_1_NoOfProperties]  DEFAULT ((0)) FOR [NoOfPropsAffected]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_NoOfZones]  DEFAULT ((0)) FOR [NoOfZones]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_NoOfCells]  DEFAULT ((0)) FOR [NoOfCells]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Properties] ADD  CONSTRAINT [DF_Properties_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Properties] ADD  CONSTRAINT [DF_Properties_IsIsolated]  DEFAULT ((0)) FOR [IsIsolated]
GO
ALTER TABLE [dbo].[Properties] ADD  CONSTRAINT [DF_Properties_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Properties] ADD  CONSTRAINT [DF_Properties_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Properties] ADD  CONSTRAINT [DF_Properties_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[PropertyStatusMstrs] ADD  CONSTRAINT [DF_StatusMstr_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PropertyStatusMstrs] ADD  CONSTRAINT [DF_PropertyStatusMstr_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PropertyStatusMstrs] ADD  CONSTRAINT [DF_PropertyStatusMstr_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[PropertySubStatusMstrs] ADD  CONSTRAINT [DF_PropertySubStatusMstrs_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PropertySubStatusMstrs] ADD  CONSTRAINT [DF_PropertySubStatusMstrs_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PropertySubStatusMstrs] ADD  CONSTRAINT [DF_PropertySubStatusMstrs_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[PropertyUserMaps] ADD  CONSTRAINT [DF_PropertyUserMaps_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PropertyUserMaps] ADD  CONSTRAINT [DF_PropertyUserMaps_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[PropertyUserMaps] ADD  CONSTRAINT [DF_PropertyUserMap_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[PropertyUserNotes] ADD  CONSTRAINT [DF_PropertyUserNotes_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PropertyUserNotes] ADD  CONSTRAINT [DF_PropertyUserNotes_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PropertyUserNotes] ADD  CONSTRAINT [DF_PropertyUserNotes_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[PropertyUserStatus] ADD  CONSTRAINT [DF_PropertyStatus_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[PropertyUserStatus] ADD  CONSTRAINT [DF_PropertyUserStatus_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PropertyUserStatus] ADD  CONSTRAINT [DF_PropertyUserStatus_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[PropertyUserStatus] ADD  CONSTRAINT [DF_PropertyUserStatus_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[RolePermissions] ADD  CONSTRAINT [DF_RolePermissions_CreatePermission]  DEFAULT ('N') FOR [CreatePermission]
GO
ALTER TABLE [dbo].[RolePermissions] ADD  CONSTRAINT [DF_RolePermissions_ReadPermission]  DEFAULT ('N') FOR [ReadPermission]
GO
ALTER TABLE [dbo].[RolePermissions] ADD  CONSTRAINT [DF_RolePermissions_UpdatePermission]  DEFAULT ('N') FOR [UpdatePermission]
GO
ALTER TABLE [dbo].[RolePermissions] ADD  CONSTRAINT [DF_RolePermissions_DeletePermission]  DEFAULT ('N') FOR [DeletePermission]
GO
ALTER TABLE [dbo].[RolePermissions] ADD  CONSTRAINT [DF_RolePermissions_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[RolePermissions] ADD  CONSTRAINT [DF_RolePermissions_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[RoleStatusMaps] ADD  CONSTRAINT [DF_RoleStatusMap_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[RoleStatusMaps] ADD  CONSTRAINT [DF_RoleStatusMap_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[RoleStatusMaps] ADD  CONSTRAINT [DF_RoleStatusMap_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[UserRoleMaps] ADD  CONSTRAINT [DF_UserRoleMap_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserRoleMaps] ADD  CONSTRAINT [DF_UserRoleMap_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[UserRoleMaps] ADD  CONSTRAINT [DF_UserRoleMap_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsAvaialbe]  DEFAULT ((0)) FOR [IsAvailable]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActiveUser]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsLoggedIn]
GO
ALTER TABLE [dbo].[AuditTrials]  WITH CHECK ADD  CONSTRAINT [FK_AuditTrial_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AuditTrials] CHECK CONSTRAINT [FK_AuditTrial_Users]
GO
ALTER TABLE [dbo].[IncidentHistories]  WITH CHECK ADD  CONSTRAINT [FK_IncidentHistory_Incidents] FOREIGN KEY([Incidentid])
REFERENCES [dbo].[Incidents] ([Id])
GO
ALTER TABLE [dbo].[IncidentHistories] CHECK CONSTRAINT [FK_IncidentHistory_Incidents]
GO
ALTER TABLE [dbo].[IncidentHistories]  WITH CHECK ADD  CONSTRAINT [FK_IncidentHistory_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[IncidentHistories] CHECK CONSTRAINT [FK_IncidentHistory_Users]
GO
ALTER TABLE [dbo].[IncidentPropsStatusCounts]  WITH CHECK ADD  CONSTRAINT [FK_IncidentPropsStatusCounts_Incidents] FOREIGN KEY([IncidentId])
REFERENCES [dbo].[Incidents] ([Id])
GO
ALTER TABLE [dbo].[IncidentPropsStatusCounts] CHECK CONSTRAINT [FK_IncidentPropsStatusCounts_Incidents]
GO
ALTER TABLE [dbo].[Incidents]  WITH CHECK ADD  CONSTRAINT [FK_Incidents_IncidentCategories] FOREIGN KEY([CategoriesMstrId])
REFERENCES [dbo].[CategoriesMstrs] ([Id])
GO
ALTER TABLE [dbo].[Incidents] CHECK CONSTRAINT [FK_Incidents_IncidentCategories]
GO
ALTER TABLE [dbo].[Properties]  WITH CHECK ADD  CONSTRAINT [FK_Properties_Incidents] FOREIGN KEY([IncidentId])
REFERENCES [dbo].[Incidents] ([Id])
GO
ALTER TABLE [dbo].[Properties] CHECK CONSTRAINT [FK_Properties_Incidents]
GO
ALTER TABLE [dbo].[Properties]  WITH CHECK ADD  CONSTRAINT [FK_Properties_PropertySubStatusMstrs] FOREIGN KEY([SubStatusId])
REFERENCES [dbo].[PropertySubStatusMstrs] ([Id])
GO
ALTER TABLE [dbo].[Properties] CHECK CONSTRAINT [FK_Properties_PropertySubStatusMstrs]
GO
ALTER TABLE [dbo].[Properties]  WITH CHECK ADD  CONSTRAINT [FK_Properties_StatusMstr] FOREIGN KEY([StatusId])
REFERENCES [dbo].[PropertyStatusMstrs] ([Id])
GO
ALTER TABLE [dbo].[Properties] CHECK CONSTRAINT [FK_Properties_StatusMstr]
GO
ALTER TABLE [dbo].[PropertySubStatusMstrs]  WITH CHECK ADD  CONSTRAINT [FK_PropertySubStatusMstrs_PropertyStatusMstrs] FOREIGN KEY([PropertyStatusMstrsId])
REFERENCES [dbo].[PropertyStatusMstrs] ([Id])
GO
ALTER TABLE [dbo].[PropertySubStatusMstrs] CHECK CONSTRAINT [FK_PropertySubStatusMstrs_PropertyStatusMstrs]
GO
ALTER TABLE [dbo].[PropertyUserMaps]  WITH CHECK ADD  CONSTRAINT [FK_PropertyUserMap_Properties] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[Properties] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserMaps] CHECK CONSTRAINT [FK_PropertyUserMap_Properties]
GO
ALTER TABLE [dbo].[PropertyUserMaps]  WITH CHECK ADD  CONSTRAINT [FK_PropertyUserMap_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserMaps] CHECK CONSTRAINT [FK_PropertyUserMap_Users]
GO
ALTER TABLE [dbo].[PropertyUserNotes]  WITH CHECK ADD  CONSTRAINT [FK_PropertyUserNotes_Properties] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[Properties] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserNotes] CHECK CONSTRAINT [FK_PropertyUserNotes_Properties]
GO
ALTER TABLE [dbo].[PropertyUserNotes]  WITH CHECK ADD  CONSTRAINT [FK_PropertyUserNotes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserNotes] CHECK CONSTRAINT [FK_PropertyUserNotes_Users]
GO
ALTER TABLE [dbo].[PropertyUserStatus]  WITH CHECK ADD  CONSTRAINT [FK_PropertyStatus_Properties] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[Properties] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserStatus] CHECK CONSTRAINT [FK_PropertyStatus_Properties]
GO
ALTER TABLE [dbo].[PropertyUserStatus]  WITH CHECK ADD  CONSTRAINT [FK_PropertyStatus_StatusMstr] FOREIGN KEY([StatusId])
REFERENCES [dbo].[PropertyStatusMstrs] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserStatus] CHECK CONSTRAINT [FK_PropertyStatus_StatusMstr]
GO
ALTER TABLE [dbo].[PropertyUserStatus]  WITH CHECK ADD  CONSTRAINT [FK_PropertyStatus_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserStatus] CHECK CONSTRAINT [FK_PropertyStatus_Users]
GO
ALTER TABLE [dbo].[PropertyUserStatus]  WITH CHECK ADD  CONSTRAINT [FK_PropertyUserStatus_PropertySubStatusMstrs] FOREIGN KEY([PropertySubStatusMstrsId])
REFERENCES [dbo].[PropertySubStatusMstrs] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserStatus] CHECK CONSTRAINT [FK_PropertyUserStatus_PropertySubStatusMstrs]
GO
ALTER TABLE [dbo].[PropertyUserStatus]  WITH CHECK ADD  CONSTRAINT [FK_PropertyUserStatus_PropertyUserMaps] FOREIGN KEY([PropertyUserMapsId])
REFERENCES [dbo].[PropertyUserMaps] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserStatus] CHECK CONSTRAINT [FK_PropertyUserStatus_PropertyUserMaps]
GO
ALTER TABLE [dbo].[PropertyUserStatus]  WITH CHECK ADD  CONSTRAINT [FK_PropertyUserStatus_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserStatus] CHECK CONSTRAINT [FK_PropertyUserStatus_Roles]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Features] FOREIGN KEY([FeatureId])
REFERENCES [dbo].[Features] ([Id])
GO
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Features]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Roles]
GO
ALTER TABLE [dbo].[RoleStatusMaps]  WITH CHECK ADD  CONSTRAINT [FK_RoleStatusMap_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[RoleStatusMaps] CHECK CONSTRAINT [FK_RoleStatusMap_Roles]
GO
ALTER TABLE [dbo].[RoleStatusMaps]  WITH CHECK ADD  CONSTRAINT [FK_RoleStatusMap_StatusMstr] FOREIGN KEY([StatusId])
REFERENCES [dbo].[PropertyStatusMstrs] ([Id])
GO
ALTER TABLE [dbo].[RoleStatusMaps] CHECK CONSTRAINT [FK_RoleStatusMap_StatusMstr]
GO
ALTER TABLE [dbo].[UserRoleMaps]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleMap_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[UserRoleMaps] CHECK CONSTRAINT [FK_UserRoleMap_Roles]
GO
ALTER TABLE [dbo].[UserRoleMaps]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleMap_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoleMaps] CHECK CONSTRAINT [FK_UserRoleMap_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Domains] FOREIGN KEY([DomainId])
REFERENCES [dbo].[Domains] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Domains]
GO
