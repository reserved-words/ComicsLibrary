CREATE PROCEDURE [ComicsLibrary].[UpdateBookHideStatus]
    @Id INT,
    @Hide BIT
AS
BEGIN

    UPDATE [ComicsLibrary].[Books]
    SET [Hidden] = @Hide
    WHERE [Id] = @Id

END
GO