CREATE TABLE [Customer].[Customer] (
    [CustomerId]			INT IDENTITY (1, 1) CONSTRAINT [DF_Customer_CustomerId] NOT NULL,
	[CustomerKey]			UniqueIdentifier	CONSTRAINT [DF_Customer_CustomerKey] DEFAULT (NewID()) NOT NULL,
	[CustomerTypeId]		INT				CONSTRAINT [DF_Customer_CustomerType] DEFAULT (-1) NOT NULL,
    [FirstName]				NVARCHAR (50)   CONSTRAINT [DF_Customer_FirstName] DEFAULT ('') NOT NULL,
    [MiddleName]			NVARCHAR (50)   CONSTRAINT [DF_Customer_MiddleName] DEFAULT ('') NOT NULL,
    [LastName]				NVARCHAR (50)   CONSTRAINT [DF_Customer_LastName] DEFAULT ('') NOT NULL,
    [BirthDate]				DATETIME        CONSTRAINT [DF_Customer_BirthDate] DEFAULT ('01-01-1900') NOT NULL,
	[GenderId]				INT				CONSTRAINT [DF_Customer_GenderISO] DEFAULT ('-1') NOT NULL,
    [ActivityContextId]		INT				CONSTRAINT [DF_Customer_ActivityContext] DEFAULT(-1) NOT NULL,
	[CreatedDate]			DATETIME        CONSTRAINT [DF_Customer_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate]			DATETIME        CONSTRAINT [DF_Customer_ModifiedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([CustomerId] ASC),
	CONSTRAINT [FK_Customer_CustomerType] FOREIGN KEY ([CustomerTypeId]) REFERENCES [Customer].[CustomerType] ([CustomerTypeId]),
	CONSTRAINT [FK_Customer_ActivityContext] FOREIGN KEY ([ActivityContextId]) REFERENCES [Activity].[ActivityContext] ([ActivityContextId]),
	CONSTRAINT [CC_Customer_Gender] CHECK ([GenderId] BETWEEN -1 AND 9)
);
GO
CREATE NonCLUSTERED INDEX [IX_Customer_All] ON [Customer].[Customer] ([FirstName] Asc, [MiddleName] Asc, [LastName] Asc, [BirthDate] Asc)
GO
CREATE UNIQUE NonCLUSTERED INDEX [IX_Customer_Key] ON [Customer].[Customer] ([CustomerKey] Asc)
GO
