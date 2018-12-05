CREATE TABLE [dbo].[RoleStatusMaps] (
    [id]         NVARCHAR (128)     CONSTRAINT [DF_RoleStatusMap_id] DEFAULT (newid()) NOT NULL,
    [RoleId]     NVARCHAR (128)     NOT NULL,
    [StatusId]   NVARCHAR (128)     NOT NULL,
    [CreatedBy]  NVARCHAR (250)     NOT NULL,
    [ModifiedBy] NVARCHAR (250)     NULL,
    [CreatedAt]  DATETIMEOFFSET (7) CONSTRAINT [DF_RoleStatusMap_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]  DATETIMEOFFSET (7) NULL,
    [Deleted]    BIT                CONSTRAINT [DF_RoleStatusMap_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]    ROWVERSION         NOT NULL,
    CONSTRAINT [PK_RoleStatusMap] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_RoleStatusMap_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id]),
    CONSTRAINT [FK_RoleStatusMap_StatusMstr] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[PropertyStatusMstrs] ([Id]),
    CONSTRAINT [IX_RoleStatusMaps] UNIQUE NONCLUSTERED ([RoleId] ASC, [StatusId] ASC)
);








GO
Create TRIGGER [dbo].[TR_dbo_RoleStatusMaps_InsertUpdateDelete] 
ON [dbo].[RoleStatusMaps] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[RoleStatusMaps] 
SET [dbo].[RoleStatusMaps].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[RoleStatusMaps].[Id] 
END;