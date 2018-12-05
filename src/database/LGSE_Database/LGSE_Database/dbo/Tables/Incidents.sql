CREATE TABLE [dbo].[Incidents] (
    [Id]                NVARCHAR (128)     CONSTRAINT [DF_Incidents_Id] DEFAULT (newid()) NOT NULL,
    [IncidentId]        NVARCHAR (50)      NOT NULL,
    [CategoriesMstrId]  NVARCHAR (128)     NOT NULL,
    [Description]       NVARCHAR (2000)    NULL,
    [Notes]             NVARCHAR (2000)    NULL,
    [NoOfPropsAffected] INT                CONSTRAINT [DF_Table_1_NoOfProperties] DEFAULT ((0)) NULL,
    [NoOfZones]         INT                CONSTRAINT [DF_Incidents_NoOfZones] DEFAULT ((0)) NULL,
    [NoOfCells]         INT                CONSTRAINT [DF_Incidents_NoOfCells] DEFAULT ((0)) NULL,
    [StartTime]         DATETIME           NULL,
    [EndTime]           DATETIME           NULL,
    [Status]            INT                CONSTRAINT [DF_Incidents_Status] DEFAULT ((0)) NULL,
    [ClosingNotes]      NVARCHAR (2000)    NULL,
    [NoOfPropsIsolated] INT                NULL,
    [NoOfPropsRestored] INT                NULL,
    [CreatedBy]         NVARCHAR (250)     NOT NULL,
    [ModifiedBy]        NVARCHAR (250)     NULL,
    [CreatedAt]         DATETIMEOFFSET (7) CONSTRAINT [DF_Incidents_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]         DATETIMEOFFSET (7) NULL,
    [Deleted]           BIT                CONSTRAINT [DF_Incidents_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]           ROWVERSION         NOT NULL,
    CONSTRAINT [PK_Incidents] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Incidents_IncidentCategories] FOREIGN KEY ([CategoriesMstrId]) REFERENCES [dbo].[CategoriesMstrs] ([Id]),
    CONSTRAINT [IX_Incidents] UNIQUE NONCLUSTERED ([IncidentId] ASC)
);












GO

CREATE INDEX [IX_Incidents_Status] ON [dbo].[Incidents] ([Status])

GO
Create TRIGGER [dbo].[TR_dbo_Incidents_InsertUpdateDelete] 
ON [dbo].[Incidents] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Incidents] 
SET [dbo].[Incidents].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Incidents].[Id] 
END;