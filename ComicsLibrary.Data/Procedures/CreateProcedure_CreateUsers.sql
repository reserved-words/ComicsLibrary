IF EXISTS (select * from sys.objects o inner join sys.schemas s on s.schema_id = o.schema_id where s.name = 'ComicsLibrary' and o.name = 'CreateUsers' and o.type = 'P') 
BEGIN
	DROP PROCEDURE [ComicsLibrary].[CreateUsers]
END
GO

CREATE PROCEDURE [ComicsLibrary].[CreateUsers]
	@DatabaseName NVARCHAR(100),
	@WebAppUser NVARCHAR(100)
AS
BEGIN
	DECLARE @SqlStatement NVARCHAR(500)

	-- Create App Pool User

	IF NOT EXISTS (SELECT LoginName FROM SYSLOGINS WHERE NAME = @WebAppUser)
	BEGIN
		SET @SqlStatement = 'CREATE LOGIN [' + @WebAppUser + '] FROM WINDOWS WITH DEFAULT_DATABASE=[' + @DatabaseName + '], DEFAULT_LANGUAGE=[us_english]'
		EXEC sp_executesql @SqlStatement
	END

	IF NOT EXISTS (SELECT [Name] FROM SYSUSERS WHERE [Name] = @WebAppUser)
	BEGIN
		SET @SqlStatement = 'CREATE USER [' + @WebAppUser + '] FOR LOGIN [' + @WebAppUser + '] WITH DEFAULT_SCHEMA = ComicsLibrary'
		EXEC sp_executesql @SqlStatement
	END

	SET @SqlStatement = 'GRANT CONNECT TO [' + @WebAppUser + ']'
	EXEC sp_executesql @SqlStatement

	SET @SqlStatement = 'GRANT SELECT, INSERT, UPDATE, DELETE ON ComicsLibrary.Comics TO [' + @WebAppUser + ']'
	EXEC sp_executesql @SqlStatement

	SET @SqlStatement = 'GRANT SELECT, INSERT, UPDATE, DELETE ON ComicsLibrary.ComicSeries TO [' + @WebAppUser + ']'
	EXEC sp_executesql @SqlStatement
END
GO