-- 1. User
CREATE TABLE [Users] (
    userID INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    email VARCHAR(200) NOT NULL,
    password VARCHAR(255) NULL,
    gender VARCHAR(20), 
    dateOfBirth DATE,
    userType VARCHAR(50) NOT NULL CHECK (userType IN ('guardian','student','tutor','child')),
    country VARCHAR(100) NULL,
    city VARCHAR(100) NULL,
    timezone VARCHAR(100) NULL,
    profilePicture VARBINARY(MAX),
    pictureType VARCHAR(50),
    createdAt DATETIME DEFAULT GETDATE()
);

-- 2. Guardian
CREATE TABLE Guardian (
    guardianID INT IDENTITY(1,1) PRIMARY KEY,
    userID INT NOT NULL,
    CONSTRAINT FK_Guardian_User FOREIGN KEY (userID) REFERENCES [Users](userID) ON DELETE CASCADE
);

-- 3. Subjects
CREATE TABLE Subjects (
    subjectID INT IDENTITY(1,1) PRIMARY KEY,
    subjectName VARCHAR(200) NOT NULL
);

-- 4. Student
CREATE TABLE Student (
    studentID INT IDENTITY(1,1) PRIMARY KEY,
    userID INT NULL,
    preferred_teacher VARCHAR(200),
    subjectID INT NOT NULL,
    createdAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Student_User FOREIGN KEY (userID) REFERENCES [Users](userID) ON DELETE CASCADE,
    CONSTRAINT FK_Student_Subject FOREIGN KEY (subjectID) REFERENCES Subjects(subjectID) ON DELETE CASCADE
);

-- 5. Child
CREATE TABLE Child (
    childID INT IDENTITY(1,1) PRIMARY KEY,
    guardianID INT NOT NULL,
    userID INT NOT NULL,
    CONSTRAINT FK_Child_Guardian FOREIGN KEY (guardianID) REFERENCES Guardian(guardianID) ON DELETE CASCADE,
    CONSTRAINT FK_Child_User FOREIGN KEY (userID) REFERENCES [Users](userID)
);

-- 6. Tutor
CREATE TABLE Tutor (
    tutorID INT IDENTITY(1,1) PRIMARY KEY,
    userID INT NOT NULL,
    about VARCHAR(MAX),
    CONSTRAINT FK_Tutor_User FOREIGN KEY (userID) REFERENCES [Users](userID) ON DELETE CASCADE
);

-- 7. Days
CREATE TABLE Days (
    dayID INT IDENTITY(1,1) PRIMARY KEY,
    dayName VARCHAR(20) NOT NULL
);

-- 8. Slots
CREATE TABLE Slots (
    slotID INT IDENTITY(1,1) PRIMARY KEY,
    startTime TIME NOT NULL,
    endTime TIME NOT NULL,
    createdAt DATETIME DEFAULT GETDATE()
);

-- 9. Certificate
CREATE TABLE Certificate (
    certificateID INT IDENTITY(1,1) PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    issued_by_department VARCHAR(200),
    image VARBINARY(MAX),
    imageType VARCHAR(20),
    tutorID INT NOT NULL,
    createdAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Certificate_Tutor FOREIGN KEY (tutorID) REFERENCES Tutor(tutorID) ON DELETE CASCADE
);

-- 10. TutorSlots
CREATE TABLE TutorSlots (
    tutorSlotID INT IDENTITY(1,1) PRIMARY KEY,
    tutorID INT NOT NULL,
    slotID INT NOT NULL,
    dayID INT NOT NULL,
    status VARCHAR(20) DEFAULT 'available',
    CONSTRAINT FK_TutorSlots_Tutor FOREIGN KEY (tutorID) REFERENCES Tutor(tutorID) ON DELETE CASCADE,
    CONSTRAINT FK_TutorSlots_Slot FOREIGN KEY (slotID) REFERENCES Slots(slotID) ON DELETE CASCADE,
    CONSTRAINT FK_TutorSlots_Day FOREIGN KEY (dayID) REFERENCES Days(dayID) ON DELETE CASCADE
);

-- 11. StudentSlots
CREATE TABLE StudentSlots (
    studentSlotID INT IDENTITY(1,1) PRIMARY KEY,
    slotID INT NOT NULL,
    studentID INT NOT NULL,
    daysID INT NOT NULL,
    CONSTRAINT FK_StudentSlots_Slot FOREIGN KEY (slotID) REFERENCES Slots(slotID) ON DELETE CASCADE,
    CONSTRAINT FK_StudentSlots_Student FOREIGN KEY (studentID) REFERENCES Student(studentID) ON DELETE CASCADE,
    CONSTRAINT FK_StudentSlots_Days FOREIGN KEY (daysID) REFERENCES Days(dayID) ON DELETE CASCADE
);

-- 12. TutorSubjects
CREATE TABLE TutorSubjects (
    tutorSubjectID INT IDENTITY(1,1) PRIMARY KEY,
    tutorID INT NOT NULL,
    subjectID INT NOT NULL,
    CONSTRAINT FK_TutorSubjects_Tutor FOREIGN KEY (tutorID) REFERENCES Tutor(tutorID) ON DELETE CASCADE,
    CONSTRAINT FK_TutorSubjects_Subject FOREIGN KEY (subjectID) REFERENCES Subjects(subjectID) ON DELETE CASCADE
);

-- 13. StudentTutorRequests
CREATE TABLE StudentTutorRequests (
    RequestID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    TutorID INT NOT NULL,
    SubjectID INT NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'pending',
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Request_Student FOREIGN KEY (StudentID) REFERENCES Student(studentID) ON DELETE CASCADE,
    CONSTRAINT FK_Tutor_Request FOREIGN KEY (TutorID) REFERENCES Tutor(tutorID),
    CONSTRAINT FK_Request_Subject FOREIGN KEY (SubjectID) REFERENCES Subjects(subjectID)
);

-- 14. Surahs
CREATE TABLE Surahs (
    SurahsID INT IDENTITY(1,1) PRIMARY KEY,
    SurahUrduName NVARCHAR(200) NOT NULL,
    SurahEngName NVARCHAR(200) NOT NULL
);

-- 15. Quran
CREATE TABLE Quran (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    VerseID INT NOT NULL,
    AyatText NVARCHAR(MAX) NOT NULL,
    SurahID INT NOT NULL,
    CONSTRAINT FK_Verses_Surah FOREIGN KEY (SurahID) REFERENCES Surahs(SurahsID) ON DELETE NO ACTION
);

-- 16. LessonPlan
CREATE TABLE LessonPlan (
    lessonPlanID INT IDENTITY(1,1) PRIMARY KEY,
    lessonName VARCHAR(200) NOT NULL
);

-- 17. Lessons
CREATE TABLE Lessons (
    LessonsID INT IDENTITY(1,1) PRIMARY KEY,
    LessonPlanID INT NOT NULL,
    QuranID INT NOT NULL,
    CONSTRAINT FK_Lessons_LessonPlan FOREIGN KEY (LessonPlanID) REFERENCES LessonPlan(lessonPlanID) ON DELETE NO ACTION,
    CONSTRAINT FK_Lessons_Quran FOREIGN KEY (QuranID) REFERENCES Quran(ID) ON DELETE NO ACTION
);

-- 18. Classes
CREATE TABLE Classes (
    ClassID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    TutorID INT NOT NULL,
    SlotID INT NOT NULL,
    SubjectID INT NOT NULL,
    LessonPlanID INT NOT NULL,
    StudentRequestTutorID INT NOT NULL,
    Status VARCHAR(50) NOT NULL DEFAULT 'Scheduled',
    Corrections VARCHAR(MAX) NULL,
    ClassDate DATE NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Classes_Student FOREIGN KEY (StudentID) REFERENCES Student(studentID),
    CONSTRAINT FK_Classes_Tutor FOREIGN KEY (TutorID) REFERENCES Tutor(tutorID),
    CONSTRAINT FK_Classes_Slot FOREIGN KEY (SlotID) REFERENCES Slots(slotID),
    CONSTRAINT FK_Classes_Subject FOREIGN KEY (SubjectID) REFERENCES Subjects(subjectID),
    CONSTRAINT FK_Classes_LessonPlan FOREIGN KEY (LessonPlanID) REFERENCES LessonPlan(lessonPlanID),
    CONSTRAINT FK_Classes_StudentRequestTutor FOREIGN KEY (StudentRequestTutorID) REFERENCES StudentTutorRequests(RequestID) ON DELETE CASCADE
);

-- 19. Reviews
CREATE TABLE Reviews (
    ReviewID INT IDENTITY(1,1) PRIMARY KEY,
    ClassID INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment VARCHAR(MAX) NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Reviews_Class FOREIGN KEY (ClassID) REFERENCES Classes(ClassID) ON DELETE CASCADE
);
