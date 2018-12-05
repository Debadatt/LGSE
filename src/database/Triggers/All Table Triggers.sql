SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create TRIGGER [dbo].[TR_dbo_AuditTrials_InsertUpdateDelete] 
ON [dbo].[AuditTrials] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[AuditTrials] 
SET [dbo].[AuditTrials].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[AuditTrials].[Id] 
END;


Create TRIGGER [dbo].[TR_dbo_CategoriesMstrs_InsertUpdateDelete] 
ON [dbo].[CategoriesMstrs] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[CategoriesMstrs] 
SET [dbo].[CategoriesMstrs].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[CategoriesMstrs].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_Domains_InsertUpdateDelete] 
ON [dbo].[Domains] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Domains] 
SET [dbo].[Domains].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Domains].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_Features_InsertUpdateDelete] 
ON [dbo].[Features] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Features] 
SET [dbo].[Features].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Features].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_IncidentHistories_InsertUpdateDelete] 
ON [dbo].[IncidentHistories] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[IncidentHistories] 
SET [dbo].[IncidentHistories].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[IncidentHistories].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_IncidentOverviewMstrs_InsertUpdateDelete] 
ON [dbo].[IncidentOverviewMstrs] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[IncidentOverviewMstrs] 
SET [dbo].[IncidentOverviewMstrs].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[IncidentOverviewMstrs].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_IncidentPropsStatusCounts_InsertUpdateDelete] 
ON [dbo].[IncidentPropsStatusCounts] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[IncidentPropsStatusCounts] 
SET [dbo].[IncidentPropsStatusCounts].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[IncidentPropsStatusCounts].[Id] 
END;


Create TRIGGER [dbo].[TR_dbo_Incidents_InsertUpdateDelete] 
ON [dbo].[Incidents] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Incidents] 
SET [dbo].[Incidents].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Incidents].[Id] 
END;


Create TRIGGER [dbo].[TR_dbo_Properties_InsertUpdateDelete] 
ON [dbo].[Properties] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Properties] 
SET [dbo].[Properties].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Properties].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_PropertyStatusMstrs_InsertUpdateDelete] 
ON [dbo].[PropertyStatusMstrs] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[PropertyStatusMstrs] 
SET [dbo].[PropertyStatusMstrs].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[PropertyStatusMstrs].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_PropertySubStatusMstrs_InsertUpdateDelete] 
ON [dbo].[PropertySubStatusMstrs] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[PropertySubStatusMstrs] 
SET [dbo].[PropertySubStatusMstrs].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[PropertySubStatusMstrs].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_PropertyUserMaps_InsertUpdateDelete] 
ON [dbo].[PropertyUserMaps] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[PropertyUserMaps] 
SET [dbo].[PropertyUserMaps].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[PropertyUserMaps].[Id] 
END;


Create TRIGGER [dbo].[TR_dbo_PropertyUserNotes_InsertUpdateDelete] 
ON [dbo].[PropertyUserNotes] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[PropertyUserNotes] 
SET [dbo].[PropertyUserNotes].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[PropertyUserNotes].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_PropertyUserStatus_InsertUpdateDelete] 
ON [dbo].[PropertyUserStatus] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[PropertyUserStatus] 
SET [dbo].[PropertyUserStatus].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[PropertyUserStatus].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_RolePermissions_InsertUpdateDelete] 
ON [dbo].[RolePermissions] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[RolePermissions] 
SET [dbo].[RolePermissions].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[RolePermissions].[Id] 
END;


Create TRIGGER [dbo].[TR_dbo_Roles_InsertUpdateDelete] 
ON [dbo].[Roles] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Roles] 
SET [dbo].[Roles].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Roles].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_RoleStatusMaps_InsertUpdateDelete] 
ON [dbo].[RoleStatusMaps] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[RoleStatusMaps] 
SET [dbo].[RoleStatusMaps].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[RoleStatusMaps].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_UserRoleMaps_InsertUpdateDelete] 
ON [dbo].[UserRoleMaps] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[UserRoleMaps] 
SET [dbo].[UserRoleMaps].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[UserRoleMaps].[Id] 
END;

Create TRIGGER [dbo].[TR_dbo_Users_InsertUpdateDelete] 
ON [dbo].[Users] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Users] 
SET [dbo].[Users].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Users].[Id] 
END;