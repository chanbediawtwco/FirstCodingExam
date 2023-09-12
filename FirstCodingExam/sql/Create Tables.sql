-- USE FIRST CODING EXAM DATABASE
USE FirstCodingExam

DECLARE @ApplicationDatabaseName nvarchar(128)
SET @ApplicationDatabaseName = N'FirstCodingExam'

-- CREATE USERS TABLE
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG = @ApplicationDatabaseName AND TABLE_NAME = 'Users'))
BEGIN
	CREATE TABLE Users (
		Id INT PRIMARY KEY IDENTITY,
		Firstname NVARCHAR(255) NOT NULL,
		Lastname NVARCHAR(255) NOT NULL,
		Email NVARCHAR(255) NOT NULL,
		Password NVARCHAR(255) NOT NULL,
	)
END

-- CREATE RECORDS TABLE
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG = @ApplicationDatabaseName AND TABLE_NAME = 'Records'))
BEGIN
	CREATE TABLE Records (
		Id INT PRIMARY KEY IDENTITY,
		UserId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
		Amount FLOAT NOT NULL,
		LowerBoundInterestRate INT NOT NULL,
		UpperBoundInterestRate INT NOT NULL,
		IncrementalRate INT NOT NULL,
		MaturityYears INT NOT NULL,
		DateCreated DATETIME NOT NULL,
		IsDeleted BIT DEFAULT 0 NOT NULL
	)
END

-- CREATE RECORDS CALCULATED TABLE
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG = @ApplicationDatabaseName AND TABLE_NAME = 'CalculatedRecords'))
BEGIN
	CREATE TABLE CalculatedRecords (
		Id INT PRIMARY KEY IDENTITY,
		RecordId INT FOREIGN KEY REFERENCES Records(Id) NOT NULL,
		Years INT NOT NULL,
		CurrentAmount FLOAT NOT NULL,
		InterestRate INT NOT NULL,
		FutureAmount FLOAT NOT NULL,
		DateCreated DATETIME NOT NULL
	)
END

-- CREATE RECORDS HISTORY TABLE
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG = @ApplicationDatabaseName AND TABLE_NAME = 'HistoryRecords'))
BEGIN
	CREATE TABLE HistoryRecords (
		Id INT PRIMARY KEY IDENTITY,
		RecordId INT FOREIGN KEY REFERENCES Records(Id) NOT NULL,
		UserId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
		Amount FLOAT NOT NULL,
		LowerBoundInterestRate INT NOT NULL,
		UpperBoundInterestRate INT NOT NULL,
		IncrementalRate INT NOT NULL,
		MaturityYears INT NOT NULL,
		DateCreated DATETIME NOT NULL
	)
END