CREATE TABLE [dbo].[AuditTrials] (
    [Id]                 NVARCHAR (128)     CONSTRAINT [DF__AuditTrials__Id__00AA174D] DEFAULT (newid()) NOT NULL,
    [UserId]             NVARCHAR (128)     NOT NULL,
    [RoleId]             NVARCHAR (128)     NULL,
    [DeviceId]           NVARCHAR (128)     NULL,
    [TypeofOperation]    NVARCHAR (128)     NULL,
    [Status]             NVARCHAR (32)      NULL,
    [OperationTimeStamp] DATETIMEOFFSET (7) NULL,
    [CreatedBy]          NVARCHAR (255)     NULL,
    [ModifiedBy]         NVARCHAR (255)     NULL,
    [Version]            ROWVERSION         NOT NULL,
    [CreatedAt]          DATETIMEOFFSET (7) CONSTRAINT [DF__AuditTria__Creat__019E3B86] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]          DATETIMEOFFSET (7) NULL,
    [Deleted]            BIT                NOT NULL,
    [IPAddress]          NVARCHAR (32)      NULL,
    CONSTRAINT [PK_dbo.AuditTrial] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AuditTrial_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);






GO
Create TRIGGER [dbo].[TR_dbo_AuditTrials_InsertUpdateDelete] 
ON [dbo].[AuditTrials] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[AuditTrials] 
SET [dbo].[AuditTrials].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[AuditTrials].[Id] 
END;