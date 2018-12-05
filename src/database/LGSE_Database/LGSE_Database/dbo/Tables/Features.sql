CREATE TABLE [dbo].[Features] (
    [Id]          NVARCHAR (128)     NOT NULL,
    [FeatureName] NVARCHAR (250)     NOT NULL,
    [FeatureText] NVARCHAR (250)     NOT NULL,
    [CreatedBy]   NVARCHAR (250)     NOT NULL,
    [ModifiedBy]  NVARCHAR (250)     NULL,
    [CreatedAt]   DATETIMEOFFSET (7) CONSTRAINT [DF_Features_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]   DATETIMEOFFSET (7) NULL,
    [Deleted]     BIT                CONSTRAINT [DF_Features_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]     ROWVERSION         NOT NULL,
    CONSTRAINT [PK_Features] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Features] UNIQUE NONCLUSTERED ([FeatureName] ASC)
);










GO
Create TRIGGER [dbo].[TR_dbo_Features_InsertUpdateDelete] 
ON [dbo].[Features] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Features] 
SET [dbo].[Features].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Features].[Id] 
END;