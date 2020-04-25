IF EXISTS (
	SELECT object_id
	FROM SYS.VIEWS V
		INNER JOIN SYS.SCHEMAS S ON S.schema_id = V.schema_id
	WHERE V.[Name] = 'SeriesAllBooks'
		AND S.[Name] = 'ComicsLibrary'
)
BEGIN
	DROP VIEW [ComicsLibrary].[SeriesAllBooks]
END
GO