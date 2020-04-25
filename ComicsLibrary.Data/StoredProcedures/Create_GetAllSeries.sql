CREATE PROCEDURE [ComicsLibrary].[GetAllSeries]
AS
BEGIN

	SELECT 
		S.Id,
        S.Title,
        B.ImageUrl,
        (SELECT COUNT([BookId]) FROM [ComicsLibrary].[SeriesUnreadBooks] WHERE [SeriesId] = S.[Id]) UnreadBooks,
        (SELECT COUNT([BookId]) FROM [ComicsLibrary].[SeriesAllBooks] WHERE [SeriesId] = S.[Id]) TotalBooks,
        S.SourceID,
        SRC.[Name] SourceName,
        [StartYear],
        [EndYear],
        [LastUpdated],
        [Abandoned] [Archived],
        S.[SourceItemID],
        S.[Url]
	FROM [ComicsLibrary].[Series] S
		INNER JOIN [ComicsLibrary].[SeriesAllBooks] U ON U.[SeriesId] = S.[Id]
		INNER JOIN [ComicsLibrary].[Books] B ON B.[Id] = U.[BookId]
		INNER JOIN [ComicsLibrary].[Sources] SRC ON SRC.[Id] = S.[SourceID]
	WHERE U.[Rank] = 1
	ORDER BY S.[Title]

END
GO