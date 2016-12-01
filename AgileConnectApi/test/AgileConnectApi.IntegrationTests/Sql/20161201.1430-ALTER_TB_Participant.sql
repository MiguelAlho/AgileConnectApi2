ALTER TABLE Participant
ADD 
	FirstName varchar(100), --needs to allow null for now
	LastName varchar(100);

GO

--Migrate existing data to new columns
UPDATE Participant
SET FirstName = SUBSTRING(Name, 0, CHARINDEX(' ', Name));

UPDATE Participant
SET LastName = REVERSE(SUBSTRING(REVERSE(Name), 0, CHARINDEX(' ', REVERSE(Name))));

GO

--Create a trigger to update First and last on Fullname insert
CREATE TRIGGER FillNamesWhenRecordInserted
ON Participant
INSTEAD OF INSERT AS
BEGIN
	DECLARE @name varchar(100)
	DECLARE @firstname varchar(100)
	DECLARE @lastname varchar(100)

	SELECT 
		@name = Name, 
		@firstname = FirstName, 
		@lastname = LastName 
	FROM inserted;

	IF @Name Is NOT NULL
		BEGIN
			SET @firstname = SUBSTRING(@name, 0, CHARINDEX(' ', @name));
			SET @lastname = REVERSE(SUBSTRING(REVERSE(@name), 0, CHARINDEX(' ', REVERSE(@name))));
		END
	ELSE
		BEGIN
			SET @name = @FirstName + ' ' + @LastName;			
		END
	INSERT INTO Participant (id, name, firstname, lastname)
		SELECT Id, @name, @firstName, @lastName
		FROM inserted;
	
END;

GO

CREATE TRIGGER UpdateNamesWhenRecordInserted
ON Participant
INSTEAD OF UPDATE AS
BEGIN
	DECLARE @id uniqueidentifier
	DECLARE @name varchar(100)
	DECLARE @firstname varchar(100)
	DECLARE @lastname varchar(100)

	SELECT 
		@name = Name, 
		@firstname = FirstName, 
		@lastname = LastName 
	FROM inserted;

	IF UPDATE(Name)
		BEGIN
			SET @firstname = SUBSTRING(@name, 0, CHARINDEX(' ', @name));
			SET @lastname = REVERSE(SUBSTRING(REVERSE(@name), 0, CHARINDEX(' ', REVERSE(@name))));
		END
	ELSE
		BEGIN
			SET @name = @FirstName + ' ' + @LastName;			
		END

	UPDATE Participant 
	SET
		Name = @name,
		FirstName = @firstname,
		LastName = @lastname
	WHERE Id = @Id
END;

GO

ALTER TABLE Participant
ALTER COLUMN Name varchar(200) NOT NULL;

ALTER TABLE Participant
ALTER COLUMN FirstName varchar(100) NOT NULL

ALTER TABLE Participant
ALTER COLUMN LastName varchar(100) NOT NULL

