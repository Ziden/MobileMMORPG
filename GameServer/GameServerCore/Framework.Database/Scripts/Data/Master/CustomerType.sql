--------------------------------------------------------------
-- CustomerType
--------------------------------------------------------------
MERGE INTO [Customer].[CustomerType] AS Target 
USING (VALUES 
	(N'00000000-0000-0000-0000-000000000000', N'None'),
	(N'BF3797EE-06A5-47F2-9016-369BEB21D944', N'Standard Customers'),
	(N'36B08B23-0C1D-4488-B557-69665FD666E1', N'Premium Customers'),
	('51A84CE1-4846-4A71-971A-CB610EEB4848', N'Lifetime Customers')
	)
AS Source ([CustomerTypeKey], [CustomerTypeName])
ON Target.[CustomerTypeKey] = Source.[CustomerTypeKey]
-- Update
WHEN MATCHED THEN 
	UPDATE SET [CustomerTypeName] = Source.[CustomerTypeName]
-- Insert 
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([CustomerTypeKey], [CustomerTypeName]) 
		Values (Source.[CustomerTypeKey], Source.[CustomerTypeName])
-- MASTER table, do not allow other record not defined here.
WHEN NOT MATCHED BY SOURCE THEN 
	DELETE;

-- Handle for default record for "Not Selected" state
If(Select Count(*) From [Customer].[CustomerType] Where CustomerTypeKey = '00000000-0000-0000-0000-000000000000' And CustomerTypeId <> -1) > 0
Begin
	Set identity_insert  [Customer].[CustomerType] ON
	Insert Into [Customer].[CustomerType] ([CustomerTypeId], [CustomerTypeKey], [CustomerTypeName])
		Select  -1 As [CustomerTypeId], [CustomerTypeKey], [CustomerTypeName] From [Customer].[CustomerType] where CustomerTypeKey = '00000000-0000-0000-0000-000000000000'
	Set identity_insert  [Customer].[CustomerType] OFF
	Delete From [Customer].[CustomerType] Where CustomerTypeId <> -1 And CustomerTypeKey = '00000000-0000-0000-0000-000000000000'
End
