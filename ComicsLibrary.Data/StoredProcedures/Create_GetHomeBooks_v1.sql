CREATE PROCEDURE [ComicsLibrary].[GetHomeBooks]
AS
BEGIN

	WITH [ReadableBooks] AS (
		SELECT 
			S.[Id] [SeriesId], 
			B.[Id] [BookId],
			B.[Title], 
			B.[Number], 
			B.[ImageUrl],
			T.[Name] [BookTypeName],
			T.[ID] [BookTypeID],
			DENSE_RANK() OVER (PARTITION BY S.[Id] ORDER BY B.[OnSaleDate], B.[Number], B.[SourceItemID]) [Rank]
		FROM [ComicsLibrary].[Series] S
			INNER JOIN [ComicsLibrary].[Books] B ON B.[SeriesId] = S.[Id]
				AND B.[Hidden] = 0
				AND B.[DateRead] IS NULL
			INNER JOIN [ComicsLibrary].[HomeBookTypes] H ON H.[SeriesId] = S.[Id] 
				AND H.BookTypeId = B.BookTypeID
				AND H.[Enabled] = 1
			INNER JOIN [ComicsLibrary].[BookType] T ON T.[ID] = H.[BookTypeId]
		WHERE S.[Abandoned] = 0
	)
	SELECT S.[Id] SeriesId
		  ,S.[Title] SeriesTitle
		  ,b.BookId
		  ,b.Title BookTitle
		  ,b.Number
		  ,b.ImageUrl
		  ,b.BookTypeName
		  ,b.BookTypeID
		  ,(SELECT COUNT(BookId) FROM ReadableBooks WHERE SeriesId = S.Id)
	FROM [ComicsLibrary].[Series] S
		INNER JOIN [ReadableBooks] B ON B.[SeriesId] = S.[Id]
	WHERE [Rank] = 1
	ORDER BY S.[Title]

END
GO