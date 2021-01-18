using System;
using System.ComponentModel.DataAnnotations;

namespace Exam.Validations
{
    public class BeforeDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(DateTime.Now>(DateTime)value)
            {
                return new ValidationResult("The activity must be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}