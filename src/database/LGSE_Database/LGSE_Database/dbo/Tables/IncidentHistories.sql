CREATE TABLE [dbo].[IncidentHistories] (
    [Id]         NVARCHAR (128)     CONSTRAINT [DF_IncidentHistory_Id] DEFAULT (newid()) NOT NULL,
    [UserId]     NVARCHAR (128)     NOT NULL,
    [Incidentid] NVARCHAR (128)     NOT NULL,
    [Status]     INT                NOT NULL,
    [CreatedBy]  NVARCHAR (250)     NOT NULL,
    [CreatedAt]  DATETIMEOFFSET (7) CONSTRAINT [DF_IncidentHistory_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]  DATETIMEOFFSET (7) NULL,
    [Deleted]    BIT                CONSTRAINT [DF_IncidentHistory_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]    ROWVERSION         NOT NULL,
    CONSTRAINT [PK_IncidentHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IncidentHistory_Incidents] FOREIGN KEY ([Incidentid]) REFERENCES [dbo].[Incidents] ([Id]),
    CONSTRAINT [FK_IncidentHistory_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);






GO
Create TRIGGER [dbo].[TR_dbo_IncidentHistories_InsertUpdateDelete] 
ON [dbo].[IncidentHistories] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[IncidentHistories] 
SET [dbo].[IncidentHistories].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[IncidentHistories].[Id] 
END;