USE Pontocanhoto;

SELECT * FROM Record

IF OBJECT_ID('Record') IS NULL
BEGIN
	CREATE TABLE Record (
		Id INT IDENTITY (1,1),
		TimesheetId INT NOT NULL,
		[Time] DATETIME2 NOT NULL,
		CONSTRAINT PK_Record_Id
			PRIMARY KEY (Id),
		CONSTRAINT FK_Record_Timesheet_TimesheetId FOREIGN KEY (TimesheetId)
			REFERENCES Timesheet(Id)
	);
END
