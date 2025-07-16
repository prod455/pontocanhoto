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
    DECLARE @UtcDate DATETIME2;
    DECLARE @OffsetMinutes INT

    SELECT @TimeZoneName = [Value] FROM TimeSetting WHERE Setting = 'Timezone';

    SET @UtcDate = GETUTCDATE();

    SET @OffsetMinutes = CASE @TimeZoneName
        WHEN 'Dateline Standard Time' THEN -720
        WHEN 'UTC-11' THEN -660
        WHEN 'Hawaiian Standard Time' THEN -600
        WHEN 'Alaskan Standard Time' THEN -540
        WHEN 'Pacific Standard Time' THEN -480
        WHEN 'Mountain Standard Time' THEN -420
        WHEN 'Central Standard Time' THEN -360
        WHEN 'Eastern Standard Time' THEN -300
        WHEN 'Atlantic Standard Time' THEN -240
        WHEN 'E. South America Standard Time' THEN -180
        WHEN 'Newfoundland Standard Time' THEN -210
        WHEN 'UTC' THEN 0
        WHEN 'GMT Standard Time' THEN 0
        WHEN 'W. Europe Standard Time' THEN 60
        WHEN 'Central Europe Standard Time' THEN 60
        WHEN 'Romance Standard Time' THEN 60
        WHEN 'E. Europe Standard Time' THEN 120
        WHEN 'Egypt Standard Time' THEN 120
        WHEN 'South Africa Standard Time' THEN 120
        WHEN 'Russian Standard Time' THEN 180
        WHEN 'Arabian Standard Time' THEN 180
        WHEN 'Iran Standard Time' THEN 210
        WHEN 'Afghanistan Standard Time' THEN 270
        WHEN 'Pakistan Standard Time' THEN 300
        WHEN 'India Standard Time' THEN 330
        WHEN 'Nepal Standard Time' THEN 345
        WHEN 'Bangladesh Standard Time' THEN 360
        WHEN 'Myanmar Standard Time' THEN 390
        WHEN 'SE Asia Standard Time' THEN 420
        WHEN 'China Standard Time' THEN 480
        WHEN 'Tokyo Standard Time' THEN 540
        WHEN 'AUS Central Standard Time' THEN 570
        WHEN 'AUS Eastern Standard Time' THEN 600
        WHEN 'Lord Howe Standard Time' THEN 630
        WHEN 'New Zealand Standard Time' THEN 720
        WHEN 'UTC+13' THEN 780
        WHEN 'UTC+14' THEN 840
        ELSE 0
    END;

    SELECT DATEADD(MINUTE, @OffsetMinutes, @UtcDate) AS LocalDateTime
END
