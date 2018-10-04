CREATE TABLE [Identity].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) CONSTRAINT [DF_AspNetUserClaims_UserId] DEFAULT ('') NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
	[CreatedDate]		DATETIME         CONSTRAINT [DF_AspNetUserClaims_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
	[ModifiedDate]      DATETIME         CONSTRAINT [DF_AspNetUserClaims_ModifiedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PKAspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FKAspNetUserClaimsAspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [Identity].[AspNetUserClaims]([UserId] ASC);

