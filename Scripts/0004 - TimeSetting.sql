USE Pontocanhoto;

IF OBJECT_ID('TimeSetting') IS NULL
BEGIN
	CREATE TABLE TimeSetting (
		Id INT IDENTITY(1,1),
		Setting VARCHAR(255),
		Value VARCHAR(255),
		CONSTRAINT PK_TimeSettings_Id PRIMARY KEY (ID)
	);
END

INSERT INTO TimeSetting(Setting, Value)
SELECT 'Timezone', 'E. South America Standard Time'
WHERE NOT EXISTS (SELECT 1 FROM TimeSetting WHERE Setting = 'Timezone')

CREATE OR ALTER PROCEDURE GetTimezoneDate
AS
BEGIN
	DECLARE @TimeZoneName NVARCHAR(100);
	DECLARE @SQL NVARCHAR(MAX);

    SELECT @TimeZoneName = [Value] FROM TimeSetting WHERE Setting = 'Timezone';

    SET @SQL = 'SELECT CONVERT(GETUTCDATE() AT TIME ZONE ''' + @TimeZoneName + ''')';

    EXEC sp_executesql @SQL;
END
