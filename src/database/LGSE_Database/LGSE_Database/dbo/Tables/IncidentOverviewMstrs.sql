CREATE TABLE [dbo].[IncidentOverviewMstrs] (
    [Id]          NVARCHAR (128)     NOT NULL,
    [Description] NVARCHAR (500)     NULL,
    [DefaultText] NVARCHAR (500)     NULL,
    [IsActive]    BIT                CONSTRAINT [DF_IncidentOverviewMstr_DefaultActive] DEFAULT ((0)) NOT NULL,
    [CreatedBy]   NVARCHAR (250)     NULL,
    [ModifiedBy]  NVARCHAR (250)     NULL,
    [CreatedAt]   DATETIMEOFFSET (7) CONSTRAINT [DF_IncidentOverviewMstr_CreatedAt] DEFAULT (sysutcdatetime()) NULL,
    [UpdatedAt]   DATETIMEOFFSET (7) NULL,
    [Deleted]     BIT                CONSTRAINT [DF_IncidentOverviewMstr_Deleted] DEFAULT ((0)) NULL,
    [Version]     ROWVERSION         NULL,
    CONSTRAINT [PK_IncidentOverviewMstr] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
Create TRIGGER [dbo].[TR_dbo_IncidentOverviewMstrs_InsertUpdateDelete] 
ON [dbo].[IncidentOverviewMstrs] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[IncidentOverviewMstrs] 
SET [dbo].[IncidentOverviewMstrs].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[IncidentOverviewMstrs].[Id] 
END;