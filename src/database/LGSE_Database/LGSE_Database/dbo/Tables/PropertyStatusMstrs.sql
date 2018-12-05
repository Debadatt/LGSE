CREATE TABLE [dbo].[PropertyStatusMstrs] (
    [Id]           NVARCHAR (128)     CONSTRAINT [DF_StatusMstr_Id] DEFAULT (newid()) NOT NULL,
    [Status]       NVARCHAR (250)     NULL,
    [DisplayOrder] INT                NOT NULL,
    [ShortText]    NVARCHAR (100)     NULL,
    [CreatedBy]    NVARCHAR (250)     NOT NULL,
    [ModifiedBy]   NVARCHAR (250)     NULL,
    [CreatedAt]    DATETIMEOFFSET (7) CONSTRAINT [DF_PropertyStatusMstr_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]    DATETIMEOFFSET (7) NULL,
    [Deleted]      BIT                CONSTRAINT [DF_PropertyStatusMstr_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]      ROWVERSION         NOT NULL,
    CONSTRAINT [PK_StatusMstr] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_PropertyStatusMstrs] UNIQUE NONCLUSTERED ([Status] ASC)
);








GO

CREATE INDEX [IX_PropertyStatusMstrs_Status] ON [dbo].[PropertyStatusMstrs] ([Status])

GO
Create TRIGGER [dbo].[TR_dbo_PropertyStatusMstrs_InsertUpdateDelete] 
ON [dbo].[PropertyStatusMstrs] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[PropertyStatusMstrs] 
SET [dbo].[PropertyStatusMstrs].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[PropertyStatusMstrs].[Id] 
END;