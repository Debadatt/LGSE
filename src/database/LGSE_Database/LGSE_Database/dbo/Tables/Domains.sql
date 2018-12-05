CREATE TABLE [dbo].[Domains] (
    [Id]         NVARCHAR (128)     CONSTRAINT [DF_Domains_id] DEFAULT (newid()) NOT NULL,
    [OrgName]    NVARCHAR (250)     NOT NULL,
    [DomainName] NVARCHAR (250)     NOT NULL,
    [IsActive]   BIT                CONSTRAINT [DF_Domains_IsActive] DEFAULT ((0)) NOT NULL,
    [CreatedBy]  NVARCHAR (250)     NOT NULL,
    [ModifiedBy] NVARCHAR (250)     NULL,
    [CreatedAt]  DATETIMEOFFSET (7) CONSTRAINT [DF_Domains_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]  DATETIMEOFFSET (7) NULL,
    [Deleted]    BIT                CONSTRAINT [DF_Domains_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]    ROWVERSION         NOT NULL,
    CONSTRAINT [PK_Domains] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Domains] UNIQUE NONCLUSTERED ([DomainName] ASC)
);








GO
Create TRIGGER [dbo].[TR_dbo_Domains_InsertUpdateDelete] 
ON [dbo].[Domains] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Domains] 
SET [dbo].[Domains].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Domains].[Id] 
END;