CREATE PROCEDURE [ComicsLibrary].[GetBooks]
    @SeriesId INT,
    @TypeId INT,
    @Limit INT,
    @Offset INT
AS
BEGIN

    SELECT
        [Id],
        [BookTypeID],
        [SeriesId],
        [SourceItemID],
        [Title],
        [Number],
        [Creators],
        [OnSaleDate],
        [ImageUrl],
        [ReadUrl],
        [DateAdded],
        [ReadUrlAdded],
        [DateRead],
        [Hidden]
    FROM
        [ComicsLibrary].[Books]
    WHERE
        [SeriesId] = @SeriesId
    AND
        [BookTypeID] = @TypeId
    ORDER BY
        [Number] DESC
	OFFSET 
		@Offset ROWS
	FETCH 
		FIRST @Limit ROWS ONLY

END
GO