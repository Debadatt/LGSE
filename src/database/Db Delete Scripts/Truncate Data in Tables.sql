--USE [LGSE-SQLDB-DEV]
--GO
--USE [LGSE-SQLDB-QA]
--GO

/*Truncate Data in Data Tables*/
--Truncate Data in Data Tables.
--Abhijeet 
--26-09-2018
truncate table dbo.[PropertyUserStatus] --done

delete from  dbo.[PropertyUserMaps] --done

truncate table dbo.[PropertyUserNotes] --done

delete from [dbo].[Properties]
GO

truncate table dbo.[AuditTrials]

truncate table IncidentPropsStatusCounts

delete from dbo.[Incidents]
Go

/*Truncate Data in Master Tables*/
--Truncate Data in Master Tables.
--Abhijeet 
--26-09-2018



delete from dbo.[CategoriesMstrs]

delete from dbo.[RolePermissions]

delete from dbo.[Features]

truncate table dbo.[RoleStatusMaps] --done

delete from dbo.[PropertySubStatusMstrs]

delete from dbo.[PropertyStatusMstrs]

truncate table dbo.[UserRoleMaps] --done

delete from dbo.[Roles]

delete from dbo.[Users]

delete from dbo.[Domains]





