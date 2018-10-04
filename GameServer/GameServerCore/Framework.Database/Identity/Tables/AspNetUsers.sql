CREATE TABLE [Identity].[AspNetUsers] (
    [Id]                   NVARCHAR (128)   NOT NULL CONSTRAINT [DF_AspNetUsers_Id] DEFAULT (''),    
    [Email]                NVARCHAR (256)   NULL	 CONSTRAINT [DF_AspNetUsers_Email] DEFAULT (''),
    [EmailConfirmed]       BIT              NOT NULL CONSTRAINT [DF_AspNetUsers_EmailConfirmed] DEFAULT (0),
    [PasswordHash]         NVARCHAR (MAX)   NULL,
    [SecurityStamp]        NVARCHAR (MAX)   NULL,
    [PhoneNumber]          NVARCHAR (MAX)   NULL,
    [PhoneNumberConfirmed] BIT              NOT NULL CONSTRAINT [DF_AspNetUsers_PhoneNumberConfirmed] DEFAULT (0),
    [TwoFactorEnabled]     BIT              NOT NULL CONSTRAINT [DF_AspNetUsers_TwoFactorEnabled] DEFAULT (0),
    [LockoutEndDateUtc]    DATETIME         NULL,
    [LockoutEnabled]       BIT              NOT NULL CONSTRAINT [DF_AspNetUsers_LockoutEnabled] DEFAULT (0),
    [AccessFailedCount]    INT              NOT NULL CONSTRAINT [DF_AspNetUsers_AccessFailedCount] DEFAULT ('-1'),
    [Username]             NVARCHAR (256)   NOT NULL CONSTRAINT [DF_AspNetUsers_Username] DEFAULT (''),
	[ActivityContextId]		INT				NOT NULL CONSTRAINT [DF_AspNetUsers_ActivityContext] DEFAULT ('-1'),
	[CreatedDate]			DATETIME		NOT NULL CONSTRAINT [DF_AspNetUsers_CreatedDate] DEFAULT (getutcdate()),
	[ModifiedDate]			DATETIME		NOT NULL CONSTRAINT [DF_AspNetUsers_ModifiedDate] DEFAULT (getutcdate()),
    CONSTRAINT [PKAspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_AspNetUsers_Username]
    ON [Identity].[AspNetUsers]([Username] ASC);
GO
CREATE NONCLUSTERED INDEX [IDX_AspNetUsers_Email]
    ON [Identity].[AspNetUsers]([Email] ASC);
GO