using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Exam.Validations
{
    public class PasswordVal : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex rgx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            if(rgx.IsMatch((string)value)==false)
            {
                return new ValidationResult("You need at least 1 lowercase letter, 1 UPPERCASE letter, 1 number and 1 special character.");
            }
            return ValidationResult.Success;
        }
    }
}