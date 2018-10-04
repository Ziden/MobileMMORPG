PRINT N'Environment-specific script: Production.sql';

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