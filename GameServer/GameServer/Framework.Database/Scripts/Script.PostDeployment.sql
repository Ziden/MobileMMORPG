/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------*/
 -- Master Data (this project is the only one updating the table x.)
:r .\Data\Master\CustomerType.sql
-- Shared Data (multiple projects updating same table with different x.)
-- For example - :r .\Data\Shared\SharedDataScript.sql
-- Any environment specific changes go here
:r .\Environment\$(TargetEnvironment).sql

--------------------------------------------------------------
--------------------------------------------------------------
-- Custom Script Items Start
--------------------------------------------------------------
--------------------------------------------------------------


--------------------------------------------------------------
--------------------------------------------------------------
-- Custom Script Items End
--------------------------------------------------------------
--------------------------------------------------------------
