CREATE TABLE [dbo].[Roles] (
    [Id]          NVARCHAR (128)     CONSTRAINT [DF_Roles_Id] DEFAULT (newid()) NOT NULL,
    [RoleName]    NVARCHAR (250)     NOT NULL,
    [Description] NVARCHAR (2000)    NULL,
    [CreatedBy]   NVARCHAR (250)     NOT NULL,
    [ModifiedBy]  NVARCHAR (250)     NULL,
    [CreatedAt]   DATETIMEOFFSET (7) CONSTRAINT [DF_Roles_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]   DATETIMEOFFSET (7) NULL,
    [Deleted]     BIT                CONSTRAINT [DF_Roles_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]     ROWVERSION         NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Roles] UNIQUE NONCLUSTERED ([RoleName] ASC)
);








GO

CREATE INDEX [IX_Roles_RoleName] ON [dbo].[Roles] ([RoleName])

GO
Create TRIGGER [dbo].[TR_dbo_Roles_InsertUpdateDelete] 
ON [dbo].[Roles] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Roles] 
SET [dbo].[Roles].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Roles].[Id] 
END;