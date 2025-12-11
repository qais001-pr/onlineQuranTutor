using System;

namespace webapi.Models
{
    public class StudentRequestsTutorDTO
    {
        public int studentid { get; set; }
        public int tutorid { get; set; }
        public int subjectid { get; set; }
        public string status  { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}