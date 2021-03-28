CREATE PROCEDURE [ComicsLibrary].[UpdateSeriesShelf]
    @SeriesId INT,
	@Shelf INT
AS
BEGIN

	UPDATE [ComicsLibrary].[Series]
	SET [Shelf] = @Shelf
	WHERE [Id] = @SeriesId

END
GO