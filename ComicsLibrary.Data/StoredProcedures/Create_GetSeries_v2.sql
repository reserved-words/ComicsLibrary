CREATE PROCEDURE [ComicsLibrary].[GetSeries]
    @SeriesID INT = NULL
AS
BEGIN

	SELECT 
		S.Id,
        S.Title,
        B.ImageUrl,
        P.UnreadBooks,
        P.TotalBooks,
        [Abandoned] [Archived],
		CAST(100 * CAST(P.ReadBooks AS DECIMAL) / CAST(P.TotalBooks AS DECIMAL) AS INTEGER) [Progress]
	FROM [ComicsLibrary].[Series] S
		INNER JOIN [ComicsLibrary].[SeriesProgress] P ON P.[Id] = S.[Id]
		INNER JOIN [ComicsLibrary].[SeriesAllBooks] U ON U.[SeriesId] = S.[Id]
		INNER JOIN [ComicsLibrary].[Books] B ON B.[Id] = U.[BookId]
		INNER JOIN [ComicsLibrary].[Sources] SRC ON SRC.[Id] = S.[SourceID]
	WHERE (@SeriesID IS NULL OR S.[Id] = @SeriesID)
		AND U.[Rank] = 1
	ORDER BY S.[Title]

END
GO