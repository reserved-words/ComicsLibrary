CREATE PROCEDURE [ComicsLibrary].[GetSeriesIds]
    @SourceId INT
AS
BEGIN

	SELECT 
		[SourceItemID] [SourceItemId],
		[Id] [LibraryId]
	FROM
		[ComicsLibrary].[Series]
	WHERE
		[SourceId] = @SourceId

END
GO