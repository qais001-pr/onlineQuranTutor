DECLARE @TutorID INT = 1;
DECLARE @DayID INT;
DECLARE @RandomSlot INT;

WHILE @TutorID <= 4
BEGIN
    SET @DayID = 1;

    WHILE @DayID <= 7
    BEGIN
        -- Random Slot between 25 and 47
        SET @RandomSlot = 1 + ABS(CHECKSUM(NEWID())) % (1 - 23 + 1);

        INSERT INTO TutorSlots (TutorID, SlotID, DayID)
        VALUES (@TutorID, @RandomSlot, @DayID);

        SET @DayID = @DayID + 1;
    END

    SET @TutorID = @TutorID + 1;
END
