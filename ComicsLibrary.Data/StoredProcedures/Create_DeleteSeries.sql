CREATE PROCEDURE [ComicsLibrary].[DeleteSeries]
    @SeriesId INT
AS
BEGIN

	DELETE FROM [ComicsLibrary].[HomeBookTypes]
	WHERE [SeriesId] = @SeriesId

	DELETE FROM [ComicsLibrary].[Books]
	WHERE [SeriesId] = @SeriesId

	DELETE FROM [ComicsLibrary].[Series]
	WHERE [Id] = @SeriesId

END
GO