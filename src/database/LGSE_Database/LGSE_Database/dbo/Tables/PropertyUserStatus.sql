CREATE TABLE [dbo].[PropertyUserStatus] (
    [id]                       NVARCHAR (128)     CONSTRAINT [DF_PropertyStatus_id] DEFAULT (newid()) NOT NULL,
    [PropertyId]               NVARCHAR (128)     NOT NULL,
    [StatusId]                 NVARCHAR (128)     NULL,
    [PropertySubStatusMstrsId] NVARCHAR (128)     NULL,
    [StatusChangedOn]          DATETIMEOFFSET (7) NULL,
    [UserId]                   NVARCHAR (128)     NOT NULL,
    [RoleId]                   NVARCHAR (128)     NOT NULL,
    [IncidentId]               NVARCHAR (128)     NULL,
    [PropertyUserMapsId]       NVARCHAR (128)     NULL,
    [Notes]                    NVARCHAR (2000)    NULL,
    [CreatedBy]                NVARCHAR (250)     NOT NULL,
    [ModifiedBy]               NVARCHAR (250)     NULL,
    [CreatedAt]                DATETIMEOFFSET (7) CONSTRAINT [DF_PropertyUserStatus_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]                DATETIMEOFFSET (7) CONSTRAINT [DF_PropertyUserStatus_UpdatedAt] DEFAULT (sysutcdatetime()) NULL,
    [Deleted]                  BIT                CONSTRAINT [DF_PropertyUserStatus_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]                  ROWVERSION         NOT NULL,
    CONSTRAINT [PK_PropertyStatus] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_PropertyStatus_Properties] FOREIGN KEY ([PropertyId]) REFERENCES [dbo].[Properties] ([Id]),
    CONSTRAINT [FK_PropertyStatus_StatusMstr] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[PropertyStatusMstrs] ([Id]),
    CONSTRAINT [FK_PropertyStatus_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_PropertyUserStatus_PropertySubStatusMstrs] FOREIGN KEY ([PropertySubStatusMstrsId]) REFERENCES [dbo].[PropertySubStatusMstrs] ([Id]),
    CONSTRAINT [FK_PropertyUserStatus_PropertyUserMaps] FOREIGN KEY ([PropertyUserMapsId]) REFERENCES [dbo].[PropertyUserMaps] ([Id]),
    CONSTRAINT [FK_PropertyUserStatus_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id])
);


















GO
CREATE NONCLUSTERED INDEX [IX_PropertyUserStatus]
    ON [dbo].[PropertyUserStatus]([PropertyUserMapsId] ASC);


GO

CREATE INDEX [IX_PropertyUserStatus_StatusId] ON [dbo].[PropertyUserStatus] ([StatusId])

GO

CREATE INDEX [IX_PropertyUserStatus_UserId] ON [dbo].[PropertyUserStatus] ([UserId])

GO
Create TRIGGER [dbo].[TR_dbo_PropertyUserStatus_InsertUpdateDelete] 
ON [dbo].[PropertyUserStatus] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[PropertyUserStatus] 
SET [dbo].[PropertyUserStatus].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[PropertyUserStatus].[Id] 
END;