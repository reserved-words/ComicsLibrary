CREATE PROCEDURE [ComicsLibrary].[GetHomeBooks]
	@SeriesID INT = NULL
AS
BEGIN

	SELECT 
		B.[Id]
		,S.[Id] SeriesId
		,s.Title SeriesTitle
		,CASE
		WHEN b.BookTypeID = 1 THEN CONCAT('#', CAST(b.Number AS VARCHAR))
		WHEN b.BookTypeID = 2 THEN CONCAT('Vol. ', CAST(b.Number AS VARCHAR))
		ELSE ''
		END IssueTitle
		,b.ImageUrl
		,b.ReadUrl
		,P.UnreadBooks
		,b.Creators
		,CAST(100 * CAST(P.ReadBooks AS DECIMAL) / CAST(P.TotalBooks AS DECIMAL) AS INTEGER) [Progress]
		,Pub.ShortName [Publisher]
		,Pub.Colour [Color]
	FROM [ComicsLibrary].[Series] S
		INNER JOIN [ComicsLibrary].[SeriesProgress] P ON P.[Id] = S.[Id]
		INNER JOIN [ComicsLibrary].[SeriesUnreadBooks] U ON U.[SeriesId] = S.[Id]
		INNER JOIN [ComicsLibrary].[Books] B ON B.[Id] = U.[BookId]
		LEFT JOIN [ComicsLibrary].[Publisher] Pub ON Pub.[Id] = S.[PublisherId]
	WHERE ((@SeriesID IS NULL AND S.[Shelf] = 1) OR S.[Id] = @SeriesID)
		AND U.[Rank] = 1
	ORDER BY S.[Title]

END
GO