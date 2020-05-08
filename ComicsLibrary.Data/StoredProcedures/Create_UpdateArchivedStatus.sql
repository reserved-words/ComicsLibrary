CREATE PROCEDURE [ComicsLibrary].[UpdateArchivedStatus]
    @SeriesId INT,
	@Archived BIT
AS
BEGIN

	UPDATE [ComicsLibrary].[Series]
	SET [Abandoned] = @Archived
	WHERE [Id] = @SeriesId

END
GO