CREATE PROCEDURE [ComicsLibrary].[UpdateBookReadStatus]
    @Id INT,
    @Read BIT
AS
BEGIN

    UPDATE [ComicsLibrary].[Books]
    SET [DateRead] = CASE WHEN @Read = 1 THEN GETDATE() ELSE NULL END
    WHERE [Id] = @Id

END
GO