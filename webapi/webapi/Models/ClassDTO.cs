using System;

namespace webapi.Models
{
    public class ClassDTO
    {
        public int studentID { get; set; }
        public int tutorID { get; set; }
        public int slotID { get; set; }
        public int lessonplanID { get; set; }
        public int subjectID { get; set; }
        public int studentRequestTutorID { get; set; }
        public int dayID { get; set; }
        public string status { get; set; }
        public int corrections { get; set; }
        public DateTime classDate { get; set; }
        public DateTime createdAt { get; set; }
    }


    public class UpdateClass
    {
        public int classID { get; set; }
        public string status { get; set; }
        public int corrections { get; set; }
    }
}

