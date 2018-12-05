CREATE TABLE [dbo].[PropertyUserNotes] (
    [Id]         NVARCHAR (128)     CONSTRAINT [DF_PropertyUserNotes_Id] DEFAULT (newid()) NOT NULL,
    [Notes]      NVARCHAR (2000)    NOT NULL,
    [UserId]     NVARCHAR (128)     NOT NULL,
    [PropertyId] NVARCHAR (128)     NOT NULL,
    [CreatedBy]  NVARCHAR (250)     NOT NULL,
    [ModifiedBy] NVARCHAR (250)     NULL,
    [CreatedAt]  DATETIMEOFFSET (7) CONSTRAINT [DF_PropertyUserNotes_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]  DATETIMEOFFSET (7) NULL,
    [Deleted]    BIT                CONSTRAINT [DF_PropertyUserNotes_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]    ROWVERSION         NOT NULL,
    CONSTRAINT [PK_PropertyNotes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PropertyUserNotes_Properties] FOREIGN KEY ([PropertyId]) REFERENCES [dbo].[Properties] ([Id]),
    CONSTRAINT [FK_PropertyUserNotes_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);






GO
Create TRIGGER [dbo].[TR_dbo_PropertyUserNotes_InsertUpdateDelete] 
ON [dbo].[PropertyUserNotes] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[PropertyUserNotes] 
SET [dbo].[PropertyUserNotes].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[PropertyUserNotes].[Id] 
END;