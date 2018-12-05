using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.Validations;
using LGSEApp.View;
using LGSEApp.ViewModels.Base;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LGSEApp.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private ValidatableObject<string> _oldPassword;
        public ValidatableObject<string> OldPassword
        {
            get
            {
                return _oldPassword;
            }
            set
            {
                _oldPassword = value;
                RaisePropertyChanged(() => OldPassword);
            }
        }
        private ValidatableObject<string> _password;

        public ValidatableObject<string> Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }
        private ValidatableObject<string> _confirmPassword;
        public ValidatableObject<string> ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }
        public ICommand SubmitCommand { get; private set; }// => new Command(() => OnSubmit(), canExecute);
        public ICommand ValidateOldPasswordCommand { get; private set; }// => new Command(() => ValidateOldPassword());
        public ICommand ValidatePasswordCommand { get; private set; }// => new Command(() => ValidatePassword());
        public ICommand ValidateConfirmPasswordCommand { get; private set; }// => new Command(() => ValidateConfirmPassword());
        bool canExecute()
        {
            if (IsBusy == false)
                return true;
            else
                return false;
        }
        public ChangePasswordViewModel()
        {
            _oldPassword = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            _confirmPassword = new ValidatableObject<string>();
            SubmitCommand = new Command(() => OnSubmit(), () => !IsBusy);
            ValidateOldPasswordCommand = new Command(() => _oldPassword.Validate(), () => this.CanValidate);
            ValidatePasswordCommand = new Command(() => _password.Validate(), () => this.CanValidate);
            ValidateConfirmPasswordCommand = new Command(() => _confirmPassword.Validate(), () => this.CanValidate);
            AddValidations();
            this.PropertyChanged += ChangePasswordViewModel_PropertyChanged;
        }
        private void ChangePasswordViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PROP_NAME_IS_BUSY)
            {
                ((Command)SubmitCommand).ChangeCanExecute();

            }
        }
        public void OnSubmit()
        {
            Validate();
            if (IsValid)
            {
                networkAccess = Connectivity.NetworkAccess;
                if (networkAccess == NetworkAccess.Internet)
                {
                    IsBusy = true;
                    this.OnSubmitExcute();
                }
                else
                {
                    Message = ResponceCode.customErrorFunction(651, null);
                }

            }
        }

        private async void OnSubmitExcute()
        {
            try
            {
                AccountService userService = new AccountService();
                ChangePasswordModel changePassword = new ChangePasswordModel();
                changePassword.token = Convert.ToString(Application.Current.Properties["token"]);
                changePassword.NewPassword = _password.Value;
                changePassword.OldPassword = _oldPassword.Value;
                var response = await userService.ChangePassword(changePassword);
                IsBusy = false;
                if (response.message == "PWD_CHANGED")
                {
                    //await Application.Current.MainPage.Navigation.PushAsync(new MainPage(true));
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                }
                else
                    Message = response.message;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public override void Validate()
        {
            base.Validate();
            _oldPassword.Validate();
            _password.Validate();
            _confirmPassword.Validate();
            if (_confirmPassword.Validate())
            {
                if (_password.Value != _confirmPassword.Value)
                    ConfirmPassword.Errors.Add(ResponceCode.applicationErrorHandler("CONFIRMPASSWORD_VALID"));
                else
                    ConfirmPassword.Errors.Add("");
            }
            this.SetValid();
        }

        public override void SetValid()
        {
            IsValid = _oldPassword.IsValid && _password.IsValid && _confirmPassword.IsValid && (_password.Value != _confirmPassword.Value ? false : true);
        }


        private void AddValidations()
        {
            _oldPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("OLDPASSWORD_REQUIRED") });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("NEWPASSWORD_REQUIRED") });
            _password.Validations.Add(new PasswordValidator<string> { ValidationMessage = ResponceCode.applicationErrorHandler("PASSWORD_VALID") });
            _confirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("CONFIRMPASSWORD_REQUIRED")  });
        }
    }
}
