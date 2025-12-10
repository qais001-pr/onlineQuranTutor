using System;
using System.Collections.Generic;
namespace webapi.Models
{
    public class SignUpUser
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string gender { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string userType { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string timezone { get; set; }
        public byte[] profilepicturebytes { get; set; }
        public string profileType { get; set; }
        public string profile64String { get; set; }
        public List<TutorSubject> tutorSubjects { get; set; }
    }

    public class TutorSubject
    {
        public int subjectid { get; set; }
    }
    public class StudentSubject
    {
        public string preffered_teachers { get; set; }
        public int userid { get; set; }
        public int subjectid { get; set; }
    }

}