using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LGSEApp.Validations
{
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            return !string.IsNullOrWhiteSpace(str);
        }
    }
    public class EmailValidator<T> : IValidationRule<T>
    {
        const string pattern = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        public string ValidationMessage { get; set; }
     
    public bool Check(T value)
        {
            if (string.IsNullOrEmpty(value as string)) return false;
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(value as string);
        }
    }
    public class PasswordValidator<T> : IValidationRule<T>
    {
        const string pattern = @"^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$";


        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (string.IsNullOrEmpty(value as string)) return false;
            var regex = new Regex(pattern);
            return regex.IsMatch(value as string);
        }
    }
    public class NameValidator<T> : IValidationRule<T>
    {
        const string pattern = @"(^[a-zA-Z]{1,20}$)";

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (string.IsNullOrEmpty(value as string)) return false;
            var regex = new Regex(pattern);
            return regex.IsMatch(value as string);
        }
    }

    public class NumericValidator<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            int result;

            var val = value as string;
            return int.TryParse(val, out result);
        }
    }
    public class EUSRNumericValidator<T> : IValidationRule<T>
    {
        const string pattern = @"(^[0-9]{1,9}$)";

        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (string.IsNullOrEmpty(value as string)) return true;
            var regex = new Regex(pattern);
            return regex.IsMatch(value as string);
        }
    }
    public class ContactNumericValidator<T> : IValidationRule<T>
    {
       
       const string pattern = @"(^[0-9]{10,13}$)";

        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (string.IsNullOrEmpty(value as string)) return true;
            var regex = new Regex(pattern);
            return regex.IsMatch(value as string);
        }
    }
}
