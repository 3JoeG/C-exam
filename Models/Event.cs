using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Exam.Validations;

namespace Exam.Models
{
    public class Event
    {
        [Key]
        public int EventId{get;set;}

        [Required (ErrorMessage="Activity name needed")]
        [Display(Name="Title:")]

        public string EventName{get;set;}
        
        [Required(ErrorMessage="A start time is needed")]
        [Display(Name="Date/Time:")]
        [BeforeDate]
        public DateTime Start{get;set;}

        [Required(ErrorMessage="A positive duration is needed")]
        [Display(Name="Duration?")]
        [Range(0,Int32.MaxValue)]
        public int Duration {get;set;}
        [Required]
        public string Unit {get;set;}

        [Required(ErrorMessage="A description is required")]
        [Display(Name="Description:")]
        public string Desc {get;set;}
        public int UserId {get;set;}
        
        public DateTime CreatedAt {get;set;}= DateTime.Now;

        public DateTime UpdatedAt {get;set;}= DateTime.Now;
    
        public User Creator {get;set;}
        public List <Attendee> Attendees{get;set;}
    }
}
