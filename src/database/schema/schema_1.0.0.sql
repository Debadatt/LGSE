USE [master]
GO
/****** Object:  Database [LGSE-DEV]    Script Date: 8/6/2018 10:30:25 PM ******/
CREATE DATABASE [LGSE-DEV]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LGSE-DEV', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\LGSE-DEV.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LGSE-DEV_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\LGSE-DEV_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [LGSE-DEV] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LGSE-DEV].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LGSE-DEV] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LGSE-DEV] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LGSE-DEV] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LGSE-DEV] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LGSE-DEV] SET ARITHABORT OFF 
GO
ALTER DATABASE [LGSE-DEV] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LGSE-DEV] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LGSE-DEV] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LGSE-DEV] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LGSE-DEV] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LGSE-DEV] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LGSE-DEV] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LGSE-DEV] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LGSE-DEV] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LGSE-DEV] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LGSE-DEV] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LGSE-DEV] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LGSE-DEV] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LGSE-DEV] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LGSE-DEV] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LGSE-DEV] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LGSE-DEV] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LGSE-DEV] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LGSE-DEV] SET  MULTI_USER 
GO
ALTER DATABASE [LGSE-DEV] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LGSE-DEV] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LGSE-DEV] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LGSE-DEV] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LGSE-DEV] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LGSE-DEV] SET QUERY_STORE = OFF
GO
USE [LGSE-DEV]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [LGSE-DEV]
GO
/****** Object:  Table [dbo].[CategoriesMstr]    Script Date: 8/6/2018 10:30:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriesMstr](
	[Id] [nvarchar](128) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_IncidentCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Domains]    Script Date: 8/6/2018 10:30:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Domains](
	[Id] [nvarchar](128) NOT NULL,
	[OrgName] [nvarchar](250) NOT NULL,
	[DomainName] [nvarchar](250) NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_Domains] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Features]    Script Date: 8/6/2018 10:30:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Features](
	[Id] [nvarchar](128) NOT NULL,
	[FeatureName] [nvarchar](250) NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_Features] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidentHistory]    Script Date: 8/6/2018 10:30:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidentHistory](
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
/****** Object:  Table [dbo].[Incidents]    Script Date: 8/6/2018 10:30:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Incidents](
	[Id] [nvarchar](128) NOT NULL,
	[IncidentId] [nvarchar](50) NOT NULL,
	[CategoryId] [nvarchar](128) NOT NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Properties]    Script Date: 8/6/2018 10:30:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Properties](
	[Id] [nvarchar](128) NOT NULL,
	[MPRN] [nvarchar](50) NOT NULL,
	[BuildingName] [nvarchar](250) NULL,
	[SubBuildingName] [nvarchar](250) NULL,
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
	[Status] [int] NULL,
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
/****** Object:  Table [dbo].[PropertyStatusMstr]    Script Date: 8/6/2018 10:30:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyStatusMstr](
	[Id] [nvarchar](128) NOT NULL,
	[Status] [nvarchar](250) NULL,
	[DisplayOrder] [int] NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_StatusMstr] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyUserMap]    Script Date: 8/6/2018 10:30:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyUserMap](
	[Id] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[PropertyId] [nvarchar](128) NOT NULL,
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
/****** Object:  Table [dbo].[PropertyUserNotes]    Script Date: 8/6/2018 10:30:25 PM ******/
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
/****** Object:  Table [dbo].[PropertyUserStatus]    Script Date: 8/6/2018 10:30:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyUserStatus](
	[id] [nvarchar](128) NOT NULL,
	[PropertyId] [nvarchar](128) NOT NULL,
	[StatusId] [nvarchar](128) NOT NULL,
	[StatusChangedOn] [datetime2](7) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
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
/****** Object:  Table [dbo].[RolePermissions]    Script Date: 8/6/2018 10:30:26 PM ******/
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
	[Version] [timestamp] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 8/6/2018 10:30:26 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleStatusMap]    Script Date: 8/6/2018 10:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleStatusMap](
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoleMap]    Script Date: 8/6/2018 10:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleMap](
	[Id] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_UserRoleMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/6/2018 10:30:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[FirstName] [nvarchar](250) NOT NULL,
	[LastName] [nvarchar](250) NOT NULL,
	[OrganizationId] [nvarchar](128) NOT NULL,
	[EmployeeId] [nvarchar](50) NOT NULL,
	[EUSR] [nvarchar](250) NULL,
	[ContactNo] [nvarchar](15) NULL,
	[IsAvailable] [bit] NOT NULL,
	[IsActivated] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[Password] [varbinary](5) NULL,
	[Salt] [varbinary](max) NULL,
	[IsTermsAccepted] [bit] NOT NULL,
	[PwdStartDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[ModifiedBy] [nvarchar](250) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CategoriesMstr] ADD  CONSTRAINT [DF_CategoriesMstr_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[CategoriesMstr] ADD  CONSTRAINT [DF_IncidentCategories_Created]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[CategoriesMstr] ADD  CONSTRAINT [DF_CategoriesMstr_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Features] ADD  CONSTRAINT [DF_Features_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Features] ADD  CONSTRAINT [DF_Features_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[IncidentHistory] ADD  CONSTRAINT [DF_IncidentHistory_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[IncidentHistory] ADD  CONSTRAINT [DF_IncidentHistory_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[IncidentHistory] ADD  CONSTRAINT [DF_IncidentHistory_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Table_1_NoOfProperties]  DEFAULT ((0)) FOR [NoOfPropsAffected]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_NoOfZones]  DEFAULT ((0)) FOR [NoOfZones]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_NoOfCells]  DEFAULT ((0)) FOR [NoOfCells]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Incidents] ADD  CONSTRAINT [DF_Incidents_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Properties] ADD  CONSTRAINT [DF_Properties_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Properties] ADD  CONSTRAINT [DF_Properties_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Properties] ADD  CONSTRAINT [DF_Properties_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[PropertyStatusMstr] ADD  CONSTRAINT [DF_StatusMstr_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PropertyStatusMstr] ADD  CONSTRAINT [DF_PropertyStatusMstr_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PropertyStatusMstr] ADD  CONSTRAINT [DF_PropertyStatusMstr_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[PropertyUserMap] ADD  CONSTRAINT [DF_PropertyUserMap_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PropertyUserMap] ADD  CONSTRAINT [DF_PropertyUserMap_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[PropertyUserNotes] ADD  CONSTRAINT [DF_PropertyUserNotes_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PropertyUserNotes] ADD  CONSTRAINT [DF_PropertyUserNotes_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PropertyUserNotes] ADD  CONSTRAINT [DF_PropertyUserNotes_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[PropertyUserStatus] ADD  CONSTRAINT [DF_PropertyStatus_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[PropertyUserStatus] ADD  CONSTRAINT [DF_PropertyUserStatus_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
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
ALTER TABLE [dbo].[RolePermissions] ADD  CONSTRAINT [DF_RolePermissions_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[RolePermissions] ADD  CONSTRAINT [DF_RolePermissions_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[RoleStatusMap] ADD  CONSTRAINT [DF_RoleStatusMap_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[RoleStatusMap] ADD  CONSTRAINT [DF_RoleStatusMap_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[RoleStatusMap] ADD  CONSTRAINT [DF_RoleStatusMap_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[UserRoleMap] ADD  CONSTRAINT [DF_UserRoleMap_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserRoleMap] ADD  CONSTRAINT [DF_UserRoleMap_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[UserRoleMap] ADD  CONSTRAINT [DF_UserRoleMap_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsAvaialbe]  DEFAULT ((0)) FOR [IsAvailable]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreatedAt]  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[IncidentHistory]  WITH CHECK ADD  CONSTRAINT [FK_IncidentHistory_Incidents] FOREIGN KEY([Incidentid])
REFERENCES [dbo].[Incidents] ([Id])
GO
ALTER TABLE [dbo].[IncidentHistory] CHECK CONSTRAINT [FK_IncidentHistory_Incidents]
GO
ALTER TABLE [dbo].[IncidentHistory]  WITH CHECK ADD  CONSTRAINT [FK_IncidentHistory_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[IncidentHistory] CHECK CONSTRAINT [FK_IncidentHistory_Users]
GO
ALTER TABLE [dbo].[Incidents]  WITH CHECK ADD  CONSTRAINT [FK_Incidents_IncidentCategories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[CategoriesMstr] ([Id])
GO
ALTER TABLE [dbo].[Incidents] CHECK CONSTRAINT [FK_Incidents_IncidentCategories]
GO
ALTER TABLE [dbo].[Properties]  WITH CHECK ADD  CONSTRAINT [FK_Properties_Incidents] FOREIGN KEY([IncidentId])
REFERENCES [dbo].[Incidents] ([Id])
GO
ALTER TABLE [dbo].[Properties] CHECK CONSTRAINT [FK_Properties_Incidents]
GO
ALTER TABLE [dbo].[PropertyUserMap]  WITH CHECK ADD  CONSTRAINT [FK_PropertyUserMap_Properties] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[Properties] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserMap] CHECK CONSTRAINT [FK_PropertyUserMap_Properties]
GO
ALTER TABLE [dbo].[PropertyUserMap]  WITH CHECK ADD  CONSTRAINT [FK_PropertyUserMap_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserMap] CHECK CONSTRAINT [FK_PropertyUserMap_Users]
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
REFERENCES [dbo].[PropertyStatusMstr] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserStatus] CHECK CONSTRAINT [FK_PropertyStatus_StatusMstr]
GO
ALTER TABLE [dbo].[PropertyUserStatus]  WITH CHECK ADD  CONSTRAINT [FK_PropertyStatus_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[PropertyUserStatus] CHECK CONSTRAINT [FK_PropertyStatus_Users]
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
ALTER TABLE [dbo].[RoleStatusMap]  WITH CHECK ADD  CONSTRAINT [FK_RoleStatusMap_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[RoleStatusMap] CHECK CONSTRAINT [FK_RoleStatusMap_Roles]
GO
ALTER TABLE [dbo].[RoleStatusMap]  WITH CHECK ADD  CONSTRAINT [FK_RoleStatusMap_StatusMstr] FOREIGN KEY([StatusId])
REFERENCES [dbo].[PropertyStatusMstr] ([Id])
GO
ALTER TABLE [dbo].[RoleStatusMap] CHECK CONSTRAINT [FK_RoleStatusMap_StatusMstr]
GO
ALTER TABLE [dbo].[UserRoleMap]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleMap_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[UserRoleMap] CHECK CONSTRAINT [FK_UserRoleMap_Roles]
GO
ALTER TABLE [dbo].[UserRoleMap]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleMap_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoleMap] CHECK CONSTRAINT [FK_UserRoleMap_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Domains] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Domains] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Domains]
GO
USE [master]
GO
ALTER DATABASE [LGSE-DEV] SET  READ_WRITE 
GO
