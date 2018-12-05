using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.Validations;
using LGSEApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LGSEApp.ViewModels
{
    public class ForgotViewModel : ViewModelBase
    {


        private ValidatableObject<string> _email;

        public ValidatableObject<string> Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }
        public ICommand ResetPasswordCommand { get; private set; }// => new Command(async () => await OnSubmit(), canExecute);
        public ICommand ValidateEmailCommand { get; private set; }// => new Command(() => ValidateEmail());
        public ICommand SignInCommand { get; private set; }
        public ForgotViewModel()
        {
            _email = new ValidatableObject<string>();
            SignInCommand = new Command(() => OnSignIn(), () => !IsBusy);
            ResetPasswordCommand = new Command(() => OnSubmit(), () => !IsBusy);
            ValidateEmailCommand = new Command(() => _email.Validate(), () => this.CanValidate);
            AddValidations();
            this.PropertyChanged += ForgotViewModel_PropertyChanged;
        }

        public override void Validate()
        {
            base.Validate();
            _email.Validate();

            this.SetValid();
        }

        public override void SetValid()
        {
            IsValid = _email.IsValid;
        }

        private void ForgotViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PROP_NAME_IS_BUSY)
            {
                ((Command)SignInCommand).ChangeCanExecute();
                ((Command)ResetPasswordCommand).ChangeCanExecute();
            }
        }


        private void AddValidations()
        {
            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("EMAIL_REQUIRED") });
            _email.Validations.Add(new EmailValidator<string> { ValidationMessage = ResponceCode.applicationErrorHandler("EMAIL_VALID") });
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
                    this.OnSubmitExecute();
                }
                else
                {
                    Message = ResponceCode.customErrorFunction(651, null);
                }
            }
        }
        public async void OnSubmitExecute()
        {
            try
            {
                IsBusy = true;
                AccountService userService = new AccountService();
                ForgotPasswordModel forgotPassword = new ForgotPasswordModel();
                forgotPassword.EmailId = _email.Value;
                var response = await userService.SendOTP(forgotPassword);
                IsBusy = false;
                if (response.message == "OTP_GENERATED")
                {                    
                    await Application.Current.MainPage.Navigation.PushAsync(new ResetPasswordPage(_email.Value));
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
        public async void OnSignIn()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }
}
