CREATE VIEW [ComicsLibrary].[SeriesProgress]
AS
	SELECT S.[Id], COUNT(R.[BookId]) [ReadBooks], COUNT(U.[BookId]) [UnreadBooks], COUNT(A.[BookId]) [TotalBooks]
	FROM [ComicsLibrary].[Series] S
		INNER JOIN [ComicsLibrary].[SeriesAllBooks] A ON A.[SeriesId] = S.[Id]
		LEFT JOIN [ComicsLibrary].[SeriesUnreadBooks] U ON U.[SeriesId] = S.[Id] AND A.[BookId] = U.[BookId]
		LEFT JOIN [ComicsLibrary].[SeriesReadBooks] R ON R.[SeriesId] = S.[Id] AND A.[BookId] = R.[BookId]
	WHERE [Id] = 1
	GROUP BY S.[Id]
GO