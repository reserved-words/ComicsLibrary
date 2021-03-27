CREATE PROCEDURE [ComicsLibrary].[GetShelf]
    @Shelf INT
AS
BEGIN

	SELECT 
		S.Id,
        S.Title,
        B.ImageUrl,
		CAST(100 * CAST(P.ReadBooks AS DECIMAL) / CAST(P.TotalBooks AS DECIMAL) AS INTEGER) [Progress],
		S.[Abandoned] [Archived],
		PUB.ShortName PublisherIcon,
		PUB.FullName Publisher,
		PUB.Colour Color,
		S.Shelf
	FROM [ComicsLibrary].[Series] S
		INNER JOIN [ComicsLibrary].[SeriesProgress] P ON P.[Id] = S.[Id]
		INNER JOIN [ComicsLibrary].[SeriesAllBooks] U ON U.[SeriesId] = S.[Id]
		INNER JOIN [ComicsLibrary].[Books] B ON B.[Id] = U.[BookId]
		INNER JOIN [ComicsLibrary].[Sources] SRC ON SRC.[Id] = S.[SourceID]
		LEFT JOIN [ComicsLibrary].[Publisher] PUB ON PUB.[Id] = S.[PublisherId]
	WHERE U.[Rank] = 1
		AND S.[Shelf] = @Shelf
	ORDER BY S.[Title]
END
GO