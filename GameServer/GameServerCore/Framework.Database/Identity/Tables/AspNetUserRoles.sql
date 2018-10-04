CREATE TABLE [Identity].[AspNetUserRoles] (
    [UserId]		NVARCHAR (128) CONSTRAINT [DF_AspNetUserRoles_UserId] DEFAULT ('') NOT NULL,
    [RoleId]		NVARCHAR (128) CONSTRAINT [DF_AspNetUserRoles_RoleId] DEFAULT ('') NOT NULL,
	[CreatedDate]	DATETIME         CONSTRAINT [DF_AspNetUserRoles_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
	[ModifiedDate]  DATETIME         CONSTRAINT [DF_AspNetUserRoles_ModifiedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PKAspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FKAspNetUserRolesAspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FKAspNetUserRolesAspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [Identity].[AspNetUserRoles]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [Identity].[AspNetUserRoles]([RoleId] ASC);

