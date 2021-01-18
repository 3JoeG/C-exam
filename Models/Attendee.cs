using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Exam.Models
{
    public class Attendee
    {
        [Key]
        public int AttendeeId {get;set;}
        public int UserId{get;set;}
        public int EventId{get;set;}

        public User Attender{get;set;}
        public Event Party{get;set;}
    }
}