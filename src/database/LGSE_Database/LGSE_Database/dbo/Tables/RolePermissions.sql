CREATE TABLE [dbo].[RolePermissions] (
    [Id]               NVARCHAR (128)     NOT NULL,
    [CreatePermission] CHAR (1)           CONSTRAINT [DF_RolePermissions_CreatePermission] DEFAULT ('N') NOT NULL,
    [ReadPermission]   CHAR (1)           CONSTRAINT [DF_RolePermissions_ReadPermission] DEFAULT ('N') NOT NULL,
    [UpdatePermission] CHAR (1)           CONSTRAINT [DF_RolePermissions_UpdatePermission] DEFAULT ('N') NOT NULL,
    [DeletePermission] CHAR (1)           CONSTRAINT [DF_RolePermissions_DeletePermission] DEFAULT ('N') NOT NULL,
    [RoleId]           NVARCHAR (128)     NOT NULL,
    [FeatureId]        NVARCHAR (128)     NOT NULL,
    [CreatedBy]        NVARCHAR (250)     NOT NULL,
    [ModifiedBy]       NVARCHAR (250)     NULL,
    [CreatedAt]        DATETIMEOFFSET (7) CONSTRAINT [DF_RolePermissions_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]        DATETIMEOFFSET (7) NULL,
    [Deleted]          BIT                CONSTRAINT [DF_RolePermissions_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]          ROWVERSION         NOT NULL,
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolePermissions_Features] FOREIGN KEY ([FeatureId]) REFERENCES [dbo].[Features] ([Id]),
    CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id]),
    CONSTRAINT [IX_RolePermissions] UNIQUE NONCLUSTERED ([FeatureId] ASC, [RoleId] ASC)
);








GO

CREATE INDEX [IX_RolePermissions_RoleId] ON [dbo].[RolePermissions] ([RoleId])

GO
Create TRIGGER [dbo].[TR_dbo_RolePermissions_InsertUpdateDelete] 
ON [dbo].[RolePermissions] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[RolePermissions] 
SET [dbo].[RolePermissions].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[RolePermissions].[Id] 
END;