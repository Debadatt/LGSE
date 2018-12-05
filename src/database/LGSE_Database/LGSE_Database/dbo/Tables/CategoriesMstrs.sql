CREATE TABLE [dbo].[CategoriesMstrs] (
    [Id]           NVARCHAR (128)     CONSTRAINT [DF_CategoriesMstr_Id] DEFAULT (newid()) NOT NULL,
    [Category]     NVARCHAR (50)      NOT NULL,
    [Description]  NVARCHAR (2000)    NULL,
    [DisplayOrder] INT                CONSTRAINT [DF_CategoriesMstrs_DisplayOrder] DEFAULT ((0)) NOT NULL,
    [CreatedBy]    NVARCHAR (250)     NOT NULL,
    [ModifiedBy]   NVARCHAR (250)     NULL,
    [CreatedAt]    DATETIMEOFFSET (7) CONSTRAINT [DF_IncidentCategories_Created] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]    DATETIMEOFFSET (7) NULL,
    [Deleted]      BIT                CONSTRAINT [DF_CategoriesMstr_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]      ROWVERSION         NOT NULL,
    CONSTRAINT [PK_IncidentCategories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_CategoriesMstrs] UNIQUE NONCLUSTERED ([Category] ASC)
);










GO
Create TRIGGER [dbo].[TR_dbo_CategoriesMstrs_InsertUpdateDelete] 
ON [dbo].[CategoriesMstrs] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[CategoriesMstrs] 
SET [dbo].[CategoriesMstrs].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[CategoriesMstrs].[Id] 
END;