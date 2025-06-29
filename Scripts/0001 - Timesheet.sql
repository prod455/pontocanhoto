USE Pontocanhoto;

IF OBJECT_ID('Timesheet') IS NULL
BEGIN
	CREATE TABLE Timesheet (
		Id INT IDENTITY (1,1),
		PeriodId INT NULL,
		[Date] DATETIME2 NOT NULL,
		CONSTRAINT PK_Timesheet_Id
			PRIMARY KEY (Id)
	);
END
