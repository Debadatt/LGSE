CREATE TABLE [dbo].[Properties] (
    [Id]                NVARCHAR (128)     CONSTRAINT [DF_Properties_Id] DEFAULT (newid()) NOT NULL,
    [MPRN]              NVARCHAR (50)      NOT NULL,
    [BuildingName]      NVARCHAR (250)     NULL,
    [SubBuildingName]   NVARCHAR (250)     NULL,
    [MCBuildingName]    NVARCHAR (250)     NULL,
    [MCSubBuildingName] NVARCHAR (250)     NULL,
    [BuildingNumber]    NVARCHAR (50)      NULL,
    [PrincipalStreet]   NVARCHAR (250)     NULL,
    [DependentStreet]   NVARCHAR (250)     NULL,
    [PostTown]          NVARCHAR (250)     NULL,
    [LocalityName]      NVARCHAR (250)     NULL,
    [DependentLocality] NVARCHAR (250)     NULL,
    [Country]           NVARCHAR (50)      NULL,
    [Postcode]          NVARCHAR (50)      NULL,
    [PriorityCustomer]  BIT                NOT NULL,
    [Zone]              NVARCHAR (250)     NULL,
    [Cell]              NVARCHAR (250)     NULL,
    [IncidentId]        NVARCHAR (128)     NULL,
    [CellManagerId]     NVARCHAR (128)     NULL,
    [ZoneManagerId]     NVARCHAR (128)     NULL,
    [StatusId]          NVARCHAR (128)     NULL,
    [SubStatusId]       NVARCHAR (128)     NULL,
    [Status]            INT                NULL,
    [Latitude]          NVARCHAR (250)     NULL,
    [Longitude]         NVARCHAR (250)     NULL,
    [IsIsolated]        BIT                CONSTRAINT [DF_Properties_IsIsolated] DEFAULT ((0)) NULL,
    [CreatedBy]         NVARCHAR (250)     NOT NULL,
    [ModifiedBy]        NVARCHAR (250)     NULL,
    [CreatedAt]         DATETIMEOFFSET (7) CONSTRAINT [DF_Properties_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]         DATETIMEOFFSET (7) CONSTRAINT [DF_Properties_UpdatedAt] DEFAULT (sysutcdatetime()) NULL,
    [Deleted]           BIT                CONSTRAINT [DF_Properties_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]           ROWVERSION         NOT NULL,
    CONSTRAINT [PK_Properties] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Properties_Incidents] FOREIGN KEY ([IncidentId]) REFERENCES [dbo].[Incidents] ([Id]),
    CONSTRAINT [FK_Properties_PropertySubStatusMstrs] FOREIGN KEY ([SubStatusId]) REFERENCES [dbo].[PropertySubStatusMstrs] ([Id]),
    CONSTRAINT [FK_Properties_StatusMstr] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[PropertyStatusMstrs] ([Id])
);














GO

CREATE INDEX [IX_Properties_MPRN] ON [dbo].[Properties] ([MPRN])

GO

CREATE NONCLUSTERED INDEX [IX_Properties_StatusId]
    ON [dbo].[Properties]([StatusId] ASC);



GO

CREATE INDEX [IX_Properties_IncidentId] ON [dbo].[Properties] ([IncidentId])

GO
CREATE NONCLUSTERED INDEX [IX_Properties_UpdatedAt]
    ON [dbo].[Properties]([UpdatedAt] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Properties_Deleted]
    ON [dbo].[Properties]([Deleted] ASC);


GO
Create TRIGGER [dbo].[TR_dbo_Properties_InsertUpdateDelete] 
ON [dbo].[Properties] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Properties] 
SET [dbo].[Properties].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Properties].[Id] 
END;