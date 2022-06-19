using System.ComponentModel.DataAnnotations;
using ZDToolbox.Extensions;

namespace ZDToolbox.Classes
{
    public class WordAllIsLetter : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value != null && value.ToString().WordAllIsLetter() ? null : new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
    public class WordAllIsLetterOrDigitValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value != null && value.ToString().WordAllIsLetterOrDigit() ? null : new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
    public class WordAllNotLetterOrDigitValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value != null && !value.ToString().WordAllIsLetterOrDigit() ? null : new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
    public class WordIsInPersianValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value != null && value.ToString().WordIsInPersian() ? null : new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
    public class WordAllIsDigitValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value != null && !value.ToString().WordAllIsDigit() ? null : new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
    public class WordAllIsLowerValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value != null && !value.ToString().WordAllIsLower() ? null : new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
    public class WordAllIsUpperValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value != null && !value.ToString().WordAllIsUpper() ? null : new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
}
