Create View [CustomerCode].[CustomerType]
As
Select	CT.[CustomerTypeId] As [Id],
		CT.[CustomerTypeKey] As [Key],
		CT.[CustomerTypeName] As [Name], 
		CT.[CreatedDate], 
		CT.[ModifiedDate]
From	[Customer].[CustomerType] CT
