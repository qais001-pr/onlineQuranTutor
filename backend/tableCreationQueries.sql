-- 1. User Table
CREATE TABLE [User] (
    userID INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    email VARCHAR(200) NULL,
    password VARCHAR(255) NULL,
    gender VARCHAR(20), 
    dateOfBirth DATE,
    userType VARCHAR(50) NOT NULL 
        CHECK (userType IN ('guardian','student','tutor','child')),
    country VARCHAR(100) NULL,
    city VARCHAR(100) NULL,
    timezone VARCHAR(100) NULL,
    profilePicture VARBINARY(MAX),
    pictureType VARCHAR(50),
    createdAt DATETIME DEFAULT GETDATE()
);

-- 2. Guardian Table
CREATE TABLE Guardian (
    guardianID INT IDENTITY(1,1) PRIMARY KEY,
    userID INT NOT NULL,
    CONSTRAINT FK_Guardian_User FOREIGN KEY (userID) REFERENCES [User](userID)
);

-- 3. Tutor Table
CREATE TABLE Tutor (
    tutorID INT IDENTITY(1,1) PRIMARY KEY,
    userID INT NOT NULL,
    about VARCHAR(MAX),
    CONSTRAINT FK_Tutor_User FOREIGN KEY (userID) REFERENCES [User](userID)
);

-- 4. Subjects Table
CREATE TABLE Subjects (
    subjectID INT IDENTITY(1,1) PRIMARY KEY,
    subjectName VARCHAR(200) NOT NULL
);

-- 5. Days Table
CREATE TABLE Days (
    dayID INT IDENTITY(1,1) PRIMARY KEY,
    dayName VARCHAR(20) NOT NULL
);

-- 6. Slots Table
CREATE TABLE Slots (
    slotID INT IDENTITY(1,1) PRIMARY KEY,
    startTime TIME NOT NULL,
    endTime TIME NOT NULL,
    createdAt DATETIME DEFAULT GETDATE()
);

-- 7. Student Table
CREATE TABLE Student (
    studentID INT IDENTITY(1,1) PRIMARY KEY,
    userID INT NULL,
    preferred_teacher varchar(50) NULL,
    subjectID INT NOT NULL,
    slotID INT NOT NULL,
    createdAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Student_User FOREIGN KEY (userID) REFERENCES [User](userID),
    CONSTRAINT FK_Student_Subject FOREIGN KEY (subjectID) REFERENCES Subjects(subjectID),
    CONSTRAINT FK_Student_Slot FOREIGN KEY (slotID) REFERENCES Slots(slotID)
);

-- 8. Child Table
CREATE TABLE Child (
    childID INT IDENTITY(1,1) PRIMARY KEY,
    guardianID INT NOT NULL,
    studentID INT NOT NULL,
    CONSTRAINT FK_Child_Guardian FOREIGN KEY (guardianID) REFERENCES Guardian(guardianID),
    CONSTRAINT FK_Child_Student FOREIGN KEY (studentID) REFERENCES Student(studentID)
);

-- 9. Certificate Table
CREATE TABLE Certificate (
    certificateID INT IDENTITY(1,1) PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    issued_by_department VARCHAR(200),
    image VARBINARY(MAX),
    imageType VARCHAR(20),
    tutorID INT NOT NULL,
    createdAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Certificate_Tutor FOREIGN KEY (tutorID) REFERENCES Tutor(tutorID)
);

-- 10. TutorSlots Table
CREATE TABLE TutorSlots (
    tutorSlotID INT IDENTITY(1,1) PRIMARY KEY,
    tutorID INT NOT NULL,
    slotID INT NOT NULL,
    CONSTRAINT FK_TutorSlots_Tutor FOREIGN KEY (tutorID) REFERENCES Tutor(tutorID),
    CONSTRAINT FK_TutorSlots_Slot FOREIGN KEY (slotID) REFERENCES Slots(slotID)
);

-- 11. TutorDays Table
CREATE TABLE TutorDays (
    tutorDaysID INT IDENTITY(1,1) PRIMARY KEY,
    tutorID INT NOT NULL,
    dayID INT NOT NULL,
    CONSTRAINT FK_TutorDays_Tutor FOREIGN KEY (tutorID) REFERENCES Tutor(tutorID),
    CONSTRAINT FK_TutorDays_Day FOREIGN KEY (dayID) REFERENCES Days(dayID)
);

-- 12. StudentDays Table
CREATE TABLE StudentDays (
    studentDaysID INT IDENTITY(1,1) PRIMARY KEY,
    studentID INT NOT NULL,
    dayID INT NOT NULL,
    CONSTRAINT FK_StudentDays_Student FOREIGN KEY (studentID) REFERENCES Student(studentID),
    CONSTRAINT FK_StudentDays_Day FOREIGN KEY (dayID) REFERENCES Days(dayID)
);

-- 13. TutorSubjects Table
CREATE TABLE TutorSubjects (
    tutorSubjectID INT IDENTITY(1,1) PRIMARY KEY,
    tutorID INT NOT NULL,
    subjectID INT NOT NULL,
    CONSTRAINT FK_TutorSubjects_Tutor FOREIGN KEY (tutorID) REFERENCES Tutor(tutorID),
    CONSTRAINT FK_TutorSubjects_Subject FOREIGN KEY (subjectID) REFERENCES Subjects(subjectID)
);

-- 14. Request Table
CREATE TABLE [Request] (
    requestID INT IDENTITY(1,1) PRIMARY KEY,
    studentID INT NOT NULL,
    tutorID INT NOT NULL,
    status VARCHAR(50) NOT NULL,
    slotID INT NOT NULL,
    createdAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Request_Student FOREIGN KEY (studentID) REFERENCES Student(studentID),
    CONSTRAINT FK_Request_Tutor FOREIGN KEY (tutorID) REFERENCES Tutor(tutorID),
    CONSTRAINT FK_Request_Slot FOREIGN KEY (slotID) REFERENCES Slots(slotID)
);

-- 15. Class Table
CREATE TABLE Class (
    classID INT IDENTITY(1,1) PRIMARY KEY,
    studentID INT NOT NULL,
    tutorID INT NOT NULL,
    subjectID INT NOT NULL,
    slotID INT NOT NULL,
    status VARCHAR(50) NOT NULL,
    createdAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Class_Student FOREIGN KEY (studentID) REFERENCES Student(studentID),
    CONSTRAINT FK_Class_Tutor FOREIGN KEY (tutorID) REFERENCES Tutor(tutorID),
    CONSTRAINT FK_Class_Subject FOREIGN KEY (subjectID) REFERENCES Subjects(subjectID),
    CONSTRAINT FK_Class_Slot FOREIGN KEY (slotID) REFERENCES Slots(slotID)
);

-- 16. Reviews Table
CREATE TABLE Reviews (
    reviewID INT IDENTITY(1,1) PRIMARY KEY,
    classID INT NOT NULL,
    studentID INT NOT NULL,
    tutorID INT NOT NULL,
    rating INT CHECK (rating BETWEEN 1 AND 5),
    comment VARCHAR(MAX),
    createdAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Reviews_Class FOREIGN KEY (classID) REFERENCES Class(classID),
    CONSTRAINT FK_Reviews_Student FOREIGN KEY (studentID) REFERENCES Student(studentID),
    CONSTRAINT FK_Reviews_Tutor FOREIGN KEY (tutorID) REFERENCES Tutor(tutorID)
);
