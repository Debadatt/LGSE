using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace LGSEApp.Behaviors
{
    public class PasswordValidationBehavior : Behavior<Entry>
    {
        const string passwordRegex = @"^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$";
        // @"^(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";
       // const string passwordRegex1 = @"^.*(?=.{8,})(?=.*[A-Z!@#$%])(?=.*\d)[a-zA-Z0-9!@#$%]+$";
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValid = false;
            IsValid = (Regex.IsMatch(e.NewTextValue, passwordRegex));
            ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
            
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
        public static bool PasswordIsvalid(string text)
        {
            bool IsValid = false;
            IsValid = (Regex.IsMatch(text, passwordRegex));
            return IsValid;
        }
    }
}
