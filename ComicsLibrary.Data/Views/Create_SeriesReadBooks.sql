CREATE VIEW [ComicsLibrary].[SeriesReadBooks]
AS
SELECT 
	S.[Id] [SeriesId], 
	B.[Id] [BookId],
	DENSE_RANK() OVER (PARTITION BY S.[Id] ORDER BY B.[OnSaleDate], B.[Number], B.[SourceItemID]) [Rank]
FROM [ComicsLibrary].[Series] S
	INNER JOIN [ComicsLibrary].[Books] B ON B.[SeriesId] = S.[Id]
		AND B.[Hidden] = 0
		AND B.[DateRead] IS NOT NULL
	INNER JOIN [ComicsLibrary].[HomeBookTypes] H ON H.[SeriesId] = S.[Id] 
		AND H.BookTypeId = B.BookTypeID
		AND H.[Enabled] = 1
	INNER JOIN [ComicsLibrary].[BookType] T ON T.[ID] = H.[BookTypeId]
GO