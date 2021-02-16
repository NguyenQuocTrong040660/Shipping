USE shippingapp_spartronics;
GO

DROP PROCEDURE [dbo].PROC_INSERT_SHIPPING_MARK_HISTORY;
GO

CREATE PROCEDURE PROC_INSERT_SHIPPING_MARK_HISTORY (
	@RefID nvarchar(30),
	@ActionType nvarchar(30),
	@OldValue nvarchar(30),
	@NewValue nvarchar(30),
	@UserName nvarchar(30)
)
AS
	BEGIN
		BEGIN TRY 
			INSERT INTO ShippingMarkHistory (RefID, ActionType, OldValue, NewValue, UserName, UpdateTime)
			VALUES (@RefID, @ActionType, @OldValue, @NewValue, @UserName, GETDATE());
		END TRY  
		BEGIN CATCH 
			SELECT ERROR_NUMBER() AS ErrorNumber  ,ERROR_SEVERITY() AS ErrorSeverity  ,ERROR_STATE() AS ErrorState  
				,ERROR_PROCEDURE() AS ErrorProcedure  ,ERROR_LINE() AS ErrorLine  ,ERROR_MESSAGE() AS ErrorMessage; 
		END CATCH; 
	END
GO
