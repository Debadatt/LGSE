CREATE TABLE [dbo].[IncidentPropsStatusCounts] (
    [Id]         NVARCHAR (128)     CONSTRAINT [DF_IncidentPropsStatusCounts_Id] DEFAULT (newid()) NOT NULL,
    [IncidentId] NVARCHAR (128)     NULL,
    [NS]         INT                NULL,
    [NA]         INT                NULL,
    [NC]         INT                NULL,
    [IS]         INT                NULL,
    [RS]         INT                NULL,
    [CreatedBy]  NVARCHAR (250)     NOT NULL,
    [ModifiedBy] NVARCHAR (250)     NULL,
    [CreatedAt]  DATETIMEOFFSET (7) CONSTRAINT [DF_IncidentPropsStatusCounts_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]  DATETIMEOFFSET (7) CONSTRAINT [DF_IncidentPropsStatusCounts_UpdatedAt] DEFAULT (sysutcdatetime()) NULL,
    [Deleted]    BIT                CONSTRAINT [DF_IncidentPropsStatusCounts_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]    ROWVERSION         NOT NULL,
    CONSTRAINT [PK_IncidentPropsStatusCounts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IncidentPropsStatusCounts_Incidents] FOREIGN KEY ([IncidentId]) REFERENCES [dbo].[Incidents] ([Id])
);


GO
Create TRIGGER [dbo].[TR_dbo_IncidentPropsStatusCounts_InsertUpdateDelete] 
ON [dbo].[IncidentPropsStatusCounts] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[IncidentPropsStatusCounts] 
SET [dbo].[IncidentPropsStatusCounts].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[IncidentPropsStatusCounts].[Id] 
END;