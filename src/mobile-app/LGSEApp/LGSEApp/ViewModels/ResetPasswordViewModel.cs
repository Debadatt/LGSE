using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.Validations;
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
    public class ResetPasswordViewModel : ViewModelBase
    {

        private ValidatableObject<string> _otpCode;

        public ValidatableObject<string> OtpCode
        {
            get
            {
                return _otpCode;
            }
            set
            {
                _otpCode = value;
                RaisePropertyChanged(() => OtpCode);
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
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        public ICommand SubmitCommand { get; private set; }
        public ICommand ValidateOTPCodeCommand { get; private set; }
        public ICommand ValidatePasswordCommand { get; private set; }
        public ICommand ValidateConfirmPasswordCommand { get; private set; }
        public ICommand SignInCommand { get; private set; }
        public ResetPasswordViewModel(string email)
        {
            Email = email;
            _otpCode = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            _confirmPassword = new ValidatableObject<string>();
            SignInCommand = new Command(() => OnSignIn(), () => !IsBusy);
            SubmitCommand = new Command(() => OnSubmit(), () => !IsBusy);
            ValidateOTPCodeCommand = new Command(() => _otpCode.Validate(), () => this.CanValidate);
            ValidatePasswordCommand = new Command(() => _password.Validate(), () => this.CanValidate);
            ValidateConfirmPasswordCommand = new Command(() => _confirmPassword.Validate(), () => this.CanValidate);
            AddValidations();
            this.PropertyChanged += ResetPasswordViewModel_PropertyChanged;
        }
        private void ResetPasswordViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PROP_NAME_IS_BUSY)
            {
                ((Command)SignInCommand).ChangeCanExecute();
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
                ForgotPasswordModel forgotPassword = new ForgotPasswordModel();
                forgotPassword.Password = _password.Value;
                forgotPassword.OtpCode = _otpCode.Value;
                forgotPassword.EmailId = Email;
                var response = await userService.ForgotPassword(forgotPassword);
                IsBusy = false;
                if (response.message == "PWD_UPDATED")
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    Message = response.message;
                }
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
            _otpCode.Validate();
            _password.Validate();
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
            IsValid = _otpCode.IsValid && _password.IsValid && _confirmPassword.IsValid && (_password.Value != _confirmPassword.Value ? false : true);
        }

        private void AddValidations()
        {
            _otpCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("OTP_REQUIRED") });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("PASSWORD_REQUIRED") });
            _password.Validations.Add(new PasswordValidator<string> { ValidationMessage = ResponceCode.applicationErrorHandler("PASSWORD_VALID") });
            _confirmPassword.Validations.Add(new PasswordValidator<string> { ValidationMessage = ResponceCode.applicationErrorHandler("CONFIRMPASSWORD_REQUIRED") });

        }
        public async void OnSignIn()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }
}
