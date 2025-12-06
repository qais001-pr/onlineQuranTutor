-- Sample Insertion Queries for StudentTutorRequests

-- 1. Student 1 requests Tutor 2 for Subject 3
INSERT INTO StudentTutorRequests (StudentID, TutorID, SubjectID, status)
VALUES (1, 2, 3, 'pending');

-- 2. Student 2 requests Tutor 1 for Subject 2
INSERT INTO StudentTutorRequests (StudentID, TutorID, SubjectID)
VALUES (2, 1, 2);

-- 3. Student 3 requests Tutor 3 for Subject 1
INSERT INTO StudentTutorRequests (StudentID, TutorID, SubjectID, status)
VALUES (3, 3, 1, 'pending');

-- 4. Student 1 requests Tutor 1 for Subject 2
INSERT INTO StudentTutorRequests (StudentID, TutorID, SubjectID)
VALUES (1, 1, 2);

-- 5. Student 2 requests Tutor 3 for Subject 3
INSERT INTO StudentTutorRequests (StudentID, TutorID, SubjectID, status)
VALUES (2, 3, 3, 'pending');
