PRINT N'Environment-specific script: Dev.sql';

--------------------------------------------------------------
-- Customer Test Data
--------------------------------------------------------------
-- Activity required for any insert, update and delete
Declare @ActivityContextId_Dev As Int
Insert INTO [Activity].[ActivityContext] ([IdentityUserName]) Select N'SQLScript@Genesys.com' As IdentityUserName
Select	@ActivityContextId_Dev = SCOPE_IDENTITY()

MERGE INTO [Customer].[Customer] AS Target
USING (VALUES 
	(N'9A530E38-6B22-48CA-95FB-182D5A64C754', N'BF3797EE-06A5-47F2-9016-369BEB21D944', N'John', N'M', N'Smith', N'05/09/1968'),
	(N'9A530E38-6B22-48CA-95FB-182D5A64C755', N'36B08B23-0C1D-4488-B557-69665FD666E1', N'Jane', N'M', N'Fox', N'05/29/1988'),
	(N'9A530E38-6B22-48CA-95FB-182D5A64C756', N'BF3797EE-06A5-47F2-9016-369BEB21D944', N'Fox', N'M', N'Doe', N'08/20/1981'),
	(N'9A530E38-6B22-48CA-95FB-182D5A64C757', N'51A84CE1-4846-4A71-971A-CB610EEB4848', N'Zane', N'M', N'Ramirez', N'05/20/1968'),
	(N'9A530E38-6B22-48CA-95FB-182D5A64C758', N'36B08B23-0C1D-4488-B557-69665FD666E1', N'Yod', N'M', N'Stack', N'11/22/1968'),
	(N'87761D52-BB75-4A10-9178-675A89782667', N'BF3797EE-06A5-47F2-9016-369BEB21D944', N'Siva', N'N', N'Kumar', N'11/15/1976'),
	(N'ABC80489-53B3-4F6D-BDC2-135E569885C5', N'51A84CE1-4846-4A71-971A-CB610EEB4848', N'Maki', N'L', N'Ishii', N'06/30/1973'),
	(N'9A530E38-6B22-48CA-95FB-182D5A64C760', N'BF3797EE-06A5-47F2-9016-369BEB21D944', N'John', N'M', N'Pseudolamellibranchiate', N'05/09/1968'),
	(N'9A530E38-6B22-48CA-95FB-182D5A64C761', N'36B08B23-0C1D-4488-B557-69665FD666E1', N'Superacknowledgement', N'M', N'Fox', N'05/29/1988'),
	(N'9A530E38-6B22-48CA-95FB-182D5A64C762', N'BF3797EE-06A5-47F2-9016-369BEB21D944', N'Fox', N'M', N'Pneumoventriculography', N'08/20/1981'),
	(N'9A530E38-6B22-48CA-95FB-182D5A64C763', N'51A84CE1-4846-4A71-971A-CB610EEB4848', N'Formaldehydesulphoxylic', N'M', N'Ramirez', N'05/20/1968'),
	(N'9A530E38-6B22-48CA-95FB-182D5A64C764', N'36B08B23-0C1D-4488-B557-69665FD666E1', N'Yod', N'M', N'Blepharoconjunctivitis', N'11/22/1968')
	)
AS Source ([CustomerKey], [CustomerTypeKey], [FirstName], [MiddleName], [LastName], [BirthDate])
	Join [Customer].[CustomerType] CT On Source.CustomerTypeKey = CT.CustomerTypeKey
ON Target.[CustomerKey] = Source.[CustomerKey]
-- Update
WHEN MATCHED THEN 
	UPDATE SET [FirstName] = Source.[FirstName], [MiddleName] = Source.[MiddleName], [CustomerTypeId] = CT.[CustomerTypeId],
		[LastName] = Source.[LastName], [BirthDate] = Source.[BirthDate], [ActivityContextId] = @ActivityContextId_Dev
-- Insert 
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([CustomerKey], [FirstName], [MiddleName], [LastName], [BirthDate], [CustomerTypeId], [ActivityContextId])
		Values (Source.[CustomerKey], Source.[FirstName], Source.[MiddleName], Source.[LastName], Source.[BirthDate], CT.[CustomerTypeId], @ActivityContextId_Dev)
;

--------------------------------------------------------------
-- Test Username
--------------------------------------------------------------
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='AspNetUsers')
Begin
	MERGE INTO [Identity].[AspNetUsers] AS Target 
	USING (VALUES 
	(N'E593A80E-7B65-499E-9AC6-CA5CBF828E3B', N'demouser@genesysframework.com', 0, N'AAkL6HezY2CeS4GcqWqikW6oveZ6gywOxPkq0+zQpCdr+23IYABz7y0grIPcpVelkA==', N'1116ce3e-2505-41d1-b5e9-4031b5481eb4', NULL, 0, 0, NULL, 1, 0, N'demouser@genesysframework.com')
	)
	AS Source ([Id],[Email],[EmailConfirmed],[PasswordHash],[SecurityStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEndDateUtc],[LockoutEnabled],[AccessFailedCount],[UserName])
	ON Target.[Email] = Source.[Email]
	-- Update
	WHEN MATCHED THEN 
		UPDATE SET [Id]= Source.[Id], [Email] = Source.[Email], [EmailConfirmed] = Source.[EmailConfirmed],[PasswordHash] = Source.[PasswordHash],[SecurityStamp] = Source.[SecurityStamp],[PhoneNumber] = Source.[PhoneNumber],
			[PhoneNumberConfirmed] = Source.[PhoneNumberConfirmed],[TwoFactorEnabled] = Source.[TwoFactorEnabled],[LockoutEndDateUtc] = Source.[LockoutEndDateUtc],[LockoutEnabled] = Source.[LockoutEnabled],
			[AccessFailedCount]=Source.[AccessFailedCount],[UserName] = Source.[UserName]
	-- Insert 
	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id],[Email],[EmailConfirmed],[PasswordHash],[SecurityStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEndDateUtc],[LockoutEnabled],[AccessFailedCount],[UserName]) 
			Values (Source.[Id],Source.[Email],Source.[EmailConfirmed],Source.[PasswordHash],Source.[SecurityStamp],Source.[PhoneNumber],Source.[PhoneNumberConfirmed],Source.[TwoFactorEnabled],
			Source.[LockoutEndDateUtc],Source.[LockoutEnabled],Source.[AccessFailedCount],Source.[UserName]);
End