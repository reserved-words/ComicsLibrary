IF EXISTS (
	SELECT object_id 
	FROM SYS.OBJECTS O
		INNER JOIN SYS.SCHEMAS S ON S.schema_id = O.schema_id
	WHERE O.[Name] = 'UpdateArchivedStatus'
		AND S.[Name] = 'ComicsLibrary'
)
BEGIN
	DROP PROCEDURE [ComicsLibrary].[UpdateArchivedStatus]
END
GO