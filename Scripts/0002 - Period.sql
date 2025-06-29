USE Pontocanhoto;

IF OBJECT_ID('Period') IS NULL
BEGIN
	CREATE TABLE [Period] (
		Id INT IDENTITY (1,1),
		TimesheetId INT NOT NULL,
		StartDate DATETIME2 NOT NULL,
		EndDate DATETIME2 NOT NULL,
		CONSTRAINT PK_Period_Id
			PRIMARY KEY (Id),
		CONSTRAINT FK_Period_Timesheet_TimesheetId FOREIGN KEY (TimesheetId)
			REFERENCES [Period](Id)
	);
END
