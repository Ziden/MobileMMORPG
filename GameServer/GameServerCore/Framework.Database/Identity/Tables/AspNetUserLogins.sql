CREATE TABLE [Identity].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) CONSTRAINT [DF_AspNetUserLogins_LoginProvider] DEFAULT ('') NOT NULL,
    [ProviderKey]   NVARCHAR (128) CONSTRAINT [DF_AspNetUserLogins_ProviderKey] DEFAULT ('') NOT NULL,
    [UserId]        NVARCHAR (128) CONSTRAINT [DF_AspNetUserLogins_UserId] DEFAULT ('') NOT NULL,
	[CreatedDate]	DATETIME         CONSTRAINT [DF_AspNetUserLogins_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
	[ModifiedDate]  DATETIME         CONSTRAINT [DF_AspNetUserLogins_ModifiedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PKAspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FKAspNetUserLoginsAspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [Identity].[AspNetUserLogins]([UserId] ASC);

