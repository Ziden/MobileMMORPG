Create Procedure [CustomerCode].[CustomerInfoUpdate]
	@Id					int = -1,	
	@FirstName			nvarchar(50),
	@MiddleName			nvarchar(50),
	@LastName			nvarchar(50),
	@BirthDate			datetime,
	@GenderId			int,
	@CustomerTypeKey	uniqueidentifier,
	@ActivityContextId	int,
	@Key				uniqueidentifier
AS
	-- Local variables
	Declare @CustomerTypeId As int = -1
	-- Initialize
	Select	@Key			= IsNull(@Key, '00000000-0000-0000-0000-000000000000')
	Select 	@FirstName		= RTRIM(LTRIM(@FirstName))
	Select 	@MiddleName		= RTRIM(LTRIM(@MiddleName))
	Select 	@LastName		= RTRIM(LTRIM(@LastName))
	
	-- Get any missing data
	Select  @CustomerTypeId = IsNull(CustomerTypeId, -1) From [Customer].[CustomerType] Where CustomerTypeKey = @CustomerTypeKey
	-- Validate data that will be inserted/updated, and ensure basic values exist
	If ((@FirstName <> '') Or (@MiddleName <> '') Or (@LastName <> '')) And (@ActivityContextId <> -1) And (@CustomerTypeId Is Not Null)
	Begin
		-- Id and Key are both valid. Sync now.
		If (@Id <> -1) Select @Key = IsNull(CustomerKey, '00000000-0000-0000-0000-000000000000') From [Customer].[Customer] Where CustomerId = @Id
		If (@Id = -1 AND @Key <> '00000000-0000-0000-0000-000000000000') Select @Id = IsNull(CustomerId, -1) From [Customer].[Customer] Where CustomerKey = @Key
		-- Insert vs Update
		Begin Tran;
		Begin Try			
			If (@Id Is Null) Or (@Id = -1)
			Begin
				Select @Key = IsNull(NullIf(@Key, '00000000-0000-0000-0000-000000000000'), NewId())
				Insert Into [Customer].[Customer] (CustomerKey, FirstName, MiddleName, LastName, BirthDate, GenderId, CustomerTypeId, ActivityContextId)
					Values (@Key, @FirstName, @MiddleName, @LastName, @BirthDate, @GenderId, @CustomerTypeId, @ActivityContextId)	
				Select	@Id = SCOPE_IDENTITY()
			End
			Else
			Begin
				-- Create main entity record
				Update C
				Set C.FirstName				= @FirstName, 
					C.MiddleName			= @MiddleName, 
					C.LastName				= @LastName, 
					C.BirthDate				= @BirthDate, 
					C.GenderId				= @GenderId,
					C.CustomerTypeId		= @CustomerTypeId,
					C.ActivityContextId		= @ActivityContextId,
					C.ModifiedDate			= GetUTCDate()
				From	[Customer].[Customer] C
				Where	C.[CustomerId] = @Id
			End
			Commit;
		End Try
		Begin Catch
			Rollback;
			Exec [Activity].[ExceptionLogInsertByActivity] @ActivityContextId;
			Throw;
		End Catch
	End	

	-- Return data
	Select	IsNull(@Id, -1) As Id, IsNull(@Key, '00000000-0000-0000-0000-000000000000') As [Key]
