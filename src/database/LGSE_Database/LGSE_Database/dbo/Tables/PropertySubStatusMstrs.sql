CREATE TABLE [dbo].[PropertySubStatusMstrs] (
    [Id]                    NVARCHAR (128)     CONSTRAINT [DF_PropertySubStatusMstrs_Id] DEFAULT (newid()) NOT NULL,
    [SubStatus]             NVARCHAR (250)     NOT NULL,
    [PropertyStatusMstrsId] NVARCHAR (128)     NOT NULL,
    [DisplayOrder]          INT                NOT NULL,
    [CreatedBy]             NVARCHAR (250)     NOT NULL,
    [ModifiedBy]            NVARCHAR (250)     NULL,
    [CreatedAt]             DATETIMEOFFSET (7) CONSTRAINT [DF_PropertySubStatusMstrs_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]             DATETIMEOFFSET (7) NULL,
    [Deleted]               BIT                CONSTRAINT [DF_PropertySubStatusMstrs_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]               ROWVERSION         NOT NULL,
    CONSTRAINT [PK_PropertySubStatusMstrs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PropertySubStatusMstrs_PropertyStatusMstrs] FOREIGN KEY ([PropertyStatusMstrsId]) REFERENCES [dbo].[PropertyStatusMstrs] ([Id]),
    CONSTRAINT [IX_PropertySubStatusMstrs] UNIQUE NONCLUSTERED ([PropertyStatusMstrsId] ASC, [SubStatus] ASC)
);








GO
Create TRIGGER [dbo].[TR_dbo_PropertySubStatusMstrs_InsertUpdateDelete] 
ON [dbo].[PropertySubStatusMstrs] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[PropertySubStatusMstrs] 
SET [dbo].[PropertySubStatusMstrs].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[PropertySubStatusMstrs].[Id] 
END;