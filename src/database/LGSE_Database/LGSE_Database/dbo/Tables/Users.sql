CREATE TABLE [dbo].[Users] (
    [Id]              NVARCHAR (128)     CONSTRAINT [DF_Users_Id] DEFAULT (newid()) NOT NULL,
    [Email]           NVARCHAR (250)     NOT NULL,
    [FirstName]       NVARCHAR (250)     NOT NULL,
    [LastName]        NVARCHAR (250)     NOT NULL,
    [DomainId]        NVARCHAR (128)     NOT NULL,
    [EmployeeId]      NVARCHAR (50)      NULL,
    [EUSR]            NVARCHAR (250)     NULL,
    [ContactNo]       NVARCHAR (15)      NULL,
    [IsAvailable]     BIT                CONSTRAINT [DF_Users_IsAvaialbe] DEFAULT ((0)) NOT NULL,
    [IsActivated]     BIT                NOT NULL,
    [IsLocked]        BIT                NOT NULL,
    [Password]        VARBINARY (MAX)    NULL,
    [Salt]            VARBINARY (MAX)    NULL,
    [IsTermsAccepted] BIT                NOT NULL,
    [PwdStartDate]    DATETIMEOFFSET (7) NULL,
    [CreatedBy]       NVARCHAR (250)     NOT NULL,
    [ModifiedBy]      NVARCHAR (250)     NULL,
    [CreatedAt]       DATETIMEOFFSET (7) CONSTRAINT [DF_Users_CreatedAt] DEFAULT (sysutcdatetime()) NOT NULL,
    [UpdatedAt]       DATETIMEOFFSET (7) NULL,
    [Deleted]         BIT                CONSTRAINT [DF_Users_Deleted] DEFAULT ((0)) NOT NULL,
    [Version]         ROWVERSION         NOT NULL,
    [OTPGeneratedAt]  DATETIMEOFFSET (7) NULL,
    [OTPCode]         NVARCHAR (32)      NULL,
    [IsActiveUser]    BIT                DEFAULT ((1)) NOT NULL,
    [IsLoggedIn]      BIT                DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Users_Domains] FOREIGN KEY ([DomainId]) REFERENCES [dbo].[Domains] ([Id]),
    CONSTRAINT [IX_Users] UNIQUE NONCLUSTERED ([Email] ASC)
);










GO

CREATE INDEX [IX_Users_IsLoggedIn] ON [dbo].[Users] ([IsLoggedIn])

GO
Create TRIGGER [dbo].[TR_dbo_Users_InsertUpdateDelete] 
ON [dbo].[Users] AFTER INSERT, UPDATE, DELETE
 AS 
BEGIN UPDATE [dbo].[Users] 
SET [dbo].[Users].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) 
FROM INSERTED WHERE inserted.[Id] = [dbo].[Users].[Id] 
END;