CREATE PROCEDURE [ComicsLibrary].[GetSeriesWithBooks]
    @SeriesId INT
AS
BEGIN
    
    SELECT [Id], [Title]
	FROM [ComicsLibrary].[Series]
	WHERE [Id] = @SeriesId

	SELECT
		[Id],
		[ReadUrl],
		[ImageUrl],
		CASE WHEN [DateRead] IS NULL THEN 0 ELSE 1 END [IsRead],
		[Title],
		[Hidden]
	FROM [ComicsLibrary].[Books]
	WHERE [SeriesId] = @SeriesId

	SELECT B.[Id], B.[Name]
	FROM [ComicsLibrary].[HomeBookTypes] H
		INNER JOIN [ComicsLibrary].[BookType] B ON B.[ID] = H.[BookTypeId]
	WHERE [SeriesId] = @SeriesId AND [Enabled] = 1

END
GO