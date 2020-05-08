CREATE PROCEDURE [ComicsLibrary].[UpdateHomeBookType]
    @SeriesId INT,
    @BookTypeId INT,
    @Enabled BIT
AS
BEGIN

    IF NOT EXISTS (
		SELECT [Id] 
		FROM [ComicsLibrary].[HomeBookTypes]
		WHERE [SeriesId] = @SeriesId 
			AND [BookTypeId] = @BookTypeId
	)
	BEGIN
		INSERT INTO [ComicsLibrary].[HomeBookTypes] (
			[SeriesId],
			[BookTypeId],
			[Enabled]
		)
		VALUES (
			@SeriesId, 
			@BookTypeId, 
			@Enabled)
	END

	UPDATE [ComicsLibrary].[HomeBookTypes]
	SET [Enabled] = @Enabled
	WHERE [SeriesId] = @SeriesId 
		AND [BookTypeId] = @BookTypeId

END
GO