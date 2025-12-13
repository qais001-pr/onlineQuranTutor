using System;

namespace webapi.Models
{
    public class AcceptRequestFromTutorDTO
    {
        public int studentID { get; set; }
        public int tutorID { get; set; }
        public int requestID { get; set; }
        public int subjectID { get; set; }
        public int surahID { get; set; }
    }
}