using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Exam.Validations;

namespace Exam.Models
{
    public class User
    {
        [Key]
        public int UserId{get;set;}

        [Display (Name="First Name")]
        [Required(ErrorMessage="First Name must be at least 2 characters long")]
        [MinLength(2)]
        public string FName{get;set;}
    
        [Display (Name="Last Name")]
        [Required (ErrorMessage="Last Name must be at least 2 characters long")]
        [MinLength(2)]
        public string LName{get;set;}
        
        [Display (Name="Email")]
        [Required(ErrorMessage="Valid Email Required")]
        [EmailAddress]
        public string Email{get;set;}
        [Display (Name="Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Password must be 8 characters long")]
        [MinLength(8)]
        [PasswordVal]
        public string Password{get;set;}

        [NotMapped]
        [Display (Name="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare ("Password",ErrorMessage="Passwords must match")]
        [MinLength(8)]
        public string Confirm{get;set;}

        List<Event> MyEvents {get;set;} 
        List<Attendee> RSVPs {get;set;}
    }






     public class Login
    {
        [Display (Name="Email")]
        [Required(ErrorMessage="Valid Email Required")]
        [EmailAddress]
        public string Email2{get;set;}

        [Display (Name="Password")]
        [Required(ErrorMessage="Invalid Password")]
        [MinLength(8)]
        public string Password2{get;set;}
    }

    public class Home
    {
        public User UserModel {get;set;}
        public Login LoginModel {get;set;}
    }
}