CREATE TABLE [dbo].[UserRoleMaps] (
    [Id]              NVARCHAR (128)     CONSTRAINT [DF_UserRoleMap_Id] DEFAULT (newid()) NOT NULL,
    [UserId]          NVARCHAR (128)     NOT NULL,
    [RoleId]          NVARCHAR (128)     NOT NULL,
    [CreatedBy]       NVARCHAR (250)     NOT NULL,
    [ModifiedBy]      NVARCHAR (250)     NULL,
    [CreatedAt]       DATETIMEOFFSET (7) CONSTRAINT [DF_UserRoleMap_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]       DATETIMEOFFSET (7) NULL,
    [Deleted]         BIT                CONSTRAINT [DF_UserRoleMap_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]         ROWVERSION         NOT NULL,
    [IsPreferredRole] BIT                NULL,
    CONSTRAINT [PK_UserRoleMap] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserRoleMap_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id]),
    CONSTRAINT [FK_UserRoleMap_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);






GO
Create TRIGGER [dbo].[TR_dbo_UserRoleMaps_InsertUpdateDelete] 
ON [dbo].[UserRoleMaps] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[UserRoleMaps] 
SET [dbo].[UserRoleMaps].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[UserRoleMaps].[Id] 
END;