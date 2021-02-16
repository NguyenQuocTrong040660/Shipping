USE shippingapp_spartronics;
GO

DROP TRIGGER [dbo].TRG_UPDATE_SHIPPING_MARKS;
GO

CREATE TRIGGER TRG_UPDATE_SHIPPING_MARKS
ON [dbo].ShippingMarks 
AFTER UPDATE
AS
	BEGIN TRY
		SET NOCOUNT ON;

		DECLARE	@RefID nvarchar(30)
		DECLARE @ActionType nvarchar(30)
		DECLARE @OldValue nvarchar(30)
		DECLARE @NewValue nvarchar(30)
		DECLARE @UserName nvarchar(30)

		DECLARE	@NewUserName nvarchar(30)
		DECLARE @NewRevision nvarchar(30)
		DECLARE @NewQuantity nvarchar(30)
		DECLARE @NewSequence nvarchar(30)
		DECLARE @NewStatus nvarchar(30)
		DECLARE @NewNotes nvarchar(30)
		DECLARE @NewProductId nvarchar(30)
		DECLARE @NewPrintCount nvarchar(30)
		DECLARE @NewShippingRequestId nvarchar(30)

		DECLARE	@OldUserName nvarchar(30)
		DECLARE @OldRevision nvarchar(30)
		DECLARE @OldQuantity nvarchar(30)
		DECLARE @OldSequence nvarchar(30)
		DECLARE @OldStatus nvarchar(30)
		DECLARE @OldNotes nvarchar(30)
		DECLARE @OldProductId nvarchar(30)
		DECLARE @OldPrintCount nvarchar(30)
		DECLARE @OldShippingRequestId nvarchar(30)
		
		SET	@RefID = ''
		SET @ActionType  = 'UPDATE'
		SET @OldValue  = ''
		SET @NewValue  = ''
		SET @UserName  = ''

		SELECT 
			@NewShippingRequestId = [ShippingRequestId]
			,@NewRevision = [Revision]
			,@NewQuantity = [Quantity]
			,@NewSequence = [Sequence]
			,@NewStatus = [Status]
			,@NewNotes = [Notes]
			,@NewProductId = [ProductId]
			,@NewPrintCount = [PrintCount]
			,@NewUserName = [LastModifiedBy]
		FROM inserted;

		SELECT 
			@OldShippingRequestId = [ShippingRequestId]
			,@OldRevision = [Revision]
			,@OldQuantity = [Quantity]
			,@OldSequence = [Sequence]
			,@OldStatus = [Status]
			,@OldNotes = [Notes]
			,@OldProductId = [ProductId]
			,@OldPrintCount = [PrintCount]
			,@OldUserName = [LastModifiedBy]
		FROM deleted;


		IF @NewRevision <> @OldRevision
		BEGIN
			SET @ActionType = 'Updated revision';
			SET @NewValue = @NewRevision;
			SET @OldValue = @OldRevision;
			SET @UserName = @NewUserName;

			EXEC [dbo].[PROC_INSERT_SHIPPING_MARK_HISTORY] @RefID, @ActionType, @OldValue, @NewValue, @UserName;
		END

		IF @NewQuantity <> @OldQuantity
		BEGIN
			SET @ActionType = 'Updated quantity';
			SET @NewValue = @NewQuantity;
			SET @OldValue = @OldQuantity;
			SET @UserName = @NewUserName;

			EXEC [dbo].[PROC_INSERT_SHIPPING_MARK_HISTORY] @RefID, @ActionType, @OldValue, @NewValue, @UserName;
		END

		IF @NewPrintCount <> @OldPrintCount
		BEGIN
			SET @ActionType = 'Updated re-print';
			SET @NewValue = @NewPrintCount;
			SET @OldValue = @OldPrintCount;
			SET @UserName = @NewUserName;

			EXEC [dbo].[PROC_INSERT_SHIPPING_MARK_HISTORY] @RefID, @ActionType, @OldValue, @NewValue, @UserName;
		END
	END TRY
	BEGIN CATCH 
		SELECT ERROR_NUMBER() AS ErrorNumber  ,ERROR_SEVERITY() AS ErrorSeverity  ,ERROR_STATE() AS ErrorState  
			,ERROR_PROCEDURE() AS ErrorProcedure  ,ERROR_LINE() AS ErrorLine  ,ERROR_MESSAGE() AS ErrorMessage; 
	END CATCH; 
GO