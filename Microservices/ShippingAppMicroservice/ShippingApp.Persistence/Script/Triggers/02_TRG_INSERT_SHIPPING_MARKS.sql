USE shippingapp_spartronics;
GO

DROP TRIGGER [dbo].TRG_INSERT_SHIPPING_MARKS;
GO

CREATE TRIGGER TRG_INSERT_SHIPPING_MARKS
ON [dbo].ShippingMarks 
AFTER INSERT
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE	@RefID nvarchar(30)
		DECLARE @ActionType nvarchar(30)
		DECLARE @OldValue nvarchar(30)
		DECLARE @NewValue nvarchar(30)
		DECLARE @UserName nvarchar(30)

		SET @ActionType = 'INSERT';
		SET @OldValue = '';

		SELECT @RefID = [ShippingRequestId], @NewValue = [Id], @UserName = [LastModifiedBy] 
		FROM inserted;

		EXEC [dbo].[PROC_INSERT_SHIPPING_MARK_HISTORY] @RefID, @ActionType, @OldValue, @NewValue, @UserName;
	END
GO