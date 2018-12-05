CREATE TABLE [dbo].[PropertyUserMaps] (
    [Id]         NVARCHAR (128)     NOT NULL,
    [UserId]     NVARCHAR (128)     NOT NULL,
    [PropertyId] NVARCHAR (128)     NOT NULL,
    [RoleId]     NVARCHAR (128)     NULL,
    [CreatedBy]  NVARCHAR (250)     NOT NULL,
    [ModifiedBy] NVARCHAR (250)     NULL,
    [CreatedAt]  DATETIMEOFFSET (7) CONSTRAINT [DF_PropertyUserMaps_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]  DATETIMEOFFSET (7) CONSTRAINT [DF_PropertyUserMaps_UpdatedAt] DEFAULT (sysutcdatetime()) NULL,
    [Deleted]    BIT                CONSTRAINT [DF_PropertyUserMap_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]    ROWVERSION         NOT NULL,
    CONSTRAINT [PK_PropertyUserMap_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PropertyUserMap_Properties] FOREIGN KEY ([PropertyId]) REFERENCES [dbo].[Properties] ([Id]),
    CONSTRAINT [FK_PropertyUserMap_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);










GO
Create TRIGGER [dbo].[TR_dbo_PropertyUserMaps_InsertUpdateDelete] 
ON [dbo].[PropertyUserMaps] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[PropertyUserMaps] 
SET [dbo].[PropertyUserMaps].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[PropertyUserMaps].[Id] 
END;