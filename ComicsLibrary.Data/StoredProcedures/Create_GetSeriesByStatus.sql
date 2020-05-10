CREATE PROCEDURE [ComicsLibrary].[GetSeriesByStatus]
    @Status INT
AS
BEGIN

	-- Reading = 0,
	-- New = 1,
	-- Finished = 2,
	-- Archived = 3

	SELECT 
		S.Id,
        S.Title,
        B.ImageUrl,
		CAST(100 * CAST(P.ReadBooks AS DECIMAL) / CAST(P.TotalBooks AS DECIMAL) AS INTEGER) [Progress],
		S.[Abandoned] [Archived]
	FROM [ComicsLibrary].[Series] S
		INNER JOIN [ComicsLibrary].[SeriesProgress] P ON P.[Id] = S.[Id]
		INNER JOIN [ComicsLibrary].[SeriesAllBooks] U ON U.[SeriesId] = S.[Id]
		INNER JOIN [ComicsLibrary].[Books] B ON B.[Id] = U.[BookId]
		INNER JOIN [ComicsLibrary].[Sources] SRC ON SRC.[Id] = S.[SourceID]
	WHERE U.[Rank] = 1
		AND (
			(@Status = 3 AND [Abandoned] = 1)
			OR
			(@Status = 2 AND [Abandoned] = 0 AND P.[TotalBooks] = p.[ReadBooks])
			OR
			(@Status = 1 AND [Abandoned] = 0 AND P.[ReadBooks] = 0)
			OR
			(@Status = 0 AND [Abandoned] = 0 AND P.[TotalBooks] > p.[ReadBooks] AND p.[ReadBooks] > 0)
		)
	ORDER BY S.[Title]

END
GO