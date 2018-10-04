
CREATE TABLE [Customer].[CustomerType] (
    [CustomerTypeId]		INT IDENTITY (1, 1) CONSTRAINT [DF_CustomerType_CustomerId] NOT NULL,
	[CustomerTypeKey]		UniqueIdentifier CONSTRAINT [DF_CustomerType_CustomerTypeKey] DEFAULT (NewID()) NOT NULL,
    [CustomerTypeName]		NVARCHAR (50)    CONSTRAINT [DF_CustomerType_CustomerTypeName] DEFAULT ('') NOT NULL,
	[CreatedDate]  DATETIME         CONSTRAINT [DF_CustomerType_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_CustomerType_ModifiedDate] DEFAULT (getutcdate()) NOT NULL,	
    CONSTRAINT [PK_CustomerType] PRIMARY KEY CLUSTERED ([CustomerTypeId] ASC)
);
GO
CREATE NonCLUSTERED INDEX [IX_CustomerType_Key] ON [Customer].[CustomerType] ([CustomerTypeKey] Asc)
GO