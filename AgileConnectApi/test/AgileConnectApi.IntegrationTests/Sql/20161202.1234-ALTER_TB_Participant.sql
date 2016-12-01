DROP TRIGGER FillNamesWhenRecordInserted;
GO

DROP TRIGGER UpdateNamesWhenRecordInserted;
GO

ALTER TABLE Participant
DROP COLUMN Name;
GO
