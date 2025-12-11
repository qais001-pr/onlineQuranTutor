using System;

namespace webapi.Models
{
    public class AcceptRequestFromTutorDTO
    {
        public int studentID { get; set; }

        public int tutorID { get; set; }

        public int slotID { get; set; }

        public int subjectID { get; set; }

        public int lessonplanID { get; set; }
        public int dayID { get; set; }

        public int studentRequestTutorID { get; set; }

        public string status { get; set; } = "pending";

        public int corrections { get; set; } = 0;

        public DateTime classDate { get; set; }

        public string createdAt { get; set; }
    }
}