CREATE TABLE [Identity].[AspNetRoles] (
    [Id]   NVARCHAR (128) CONSTRAINT [DF_AspNetRoles_Id] DEFAULT ('') NOT NULL,
    [Name] NVARCHAR (256) CONSTRAINT [DF_AspNetRoles_Name] DEFAULT ('') NOT NULL,
	[CreatedDate]		DATETIME         CONSTRAINT [DF_AspNetRoles_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
	[ModifiedDate]      DATETIME         CONSTRAINT [DF_AspNetRoles_ModifiedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [Identity].[AspNetRoles]([Name] ASC);

