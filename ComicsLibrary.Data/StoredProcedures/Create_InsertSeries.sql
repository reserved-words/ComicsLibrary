CREATE PROCEDURE [ComicsLibrary].[InsertSeries]
    @SourceId INT,
    @SourceItemID INT,
    @Title NVARCHAR(255),
    @Url NVARCHAR(255),
    @ImageUrl NVARCHAR(255),
    @Id INT OUTPUT
AS
BEGIN

	INSERT INTO [ComicsLibrary].[Series] (
        [SourceId],
        [SourceItemID],
        [Title],
        [Url],
        [ImageUrl]
    )
    VALUES (
        @SourceId,
        @SourceItemID,
        @Title,
        @Url,
        @ImageUrl
    )

    SET @Id = SCOPE_IDENTITY()

END
GO