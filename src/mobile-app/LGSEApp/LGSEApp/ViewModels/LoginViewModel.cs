#define OFFLINE_SYNC_ENABLED
using LGSEApp.Services;
using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.Validations;
using LGSEApp.View;
using LGSEApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using Xamarin.Essentials;
using System.Diagnostics;
#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace LGSEApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        private ValidatableObject<string> _userName;
        private ValidatableObject<string> _password;
        public ValidatableObject<string> UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;

                RaisePropertyChanged(() => UserName);
            }
        }

        public ValidatableObject<string> Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;

                RaisePropertyChanged(() => Password);
            }
        }



        public ICommand SignInCommand { get; private set; } //=> new Command(async () => await OnSubmit(), canExecuteLogin);
        public ICommand ValidateUserNameCommand { get; private set; }// => new Command(() => ValidateUserName());
        public ICommand ValidatePasswordCommand { get; private set; }// => new Command(() => ValidatePassword());
        public ICommand ForgotCommand { get; private set; }// => new Command(async () => await OnForgot(), canExecute);
        public ICommand SignUpCommand { get; private set; }// => new Command(async () => await OnSignUp(), canExecute);
        public LoginViewModel()
        {
            IsLogged = false;
            _userName = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            GetUserId();
            SignInCommand = new Command(() => OnSubmit(), () => !IsBusy);
            ValidateUserNameCommand = new Command(() => _userName.Validate(), () => this.CanValidate);
            ValidatePasswordCommand = new Command(() => _password.Validate(), () => this.CanValidate);
            ForgotCommand = new Command(async () => await OnForgot(), () => !IsBusy);
            SignUpCommand = new Command(async () => await OnSignUp(), () => !IsBusy);

            AddValidations();

            this.PropertyChanged += LoginViewModel_PropertyChanged;
        }
        void GetUserId()
        {
            if (Application.Current.Properties.ContainsKey("username"))
            {
                try
                {
                    //  Debug.Write(Convert.ToString(Application.Current.Properties["username"]));
                    UserName.Value = Convert.ToString(Application.Current.Properties["username"]);
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                }
            }
        }
        private void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PROP_NAME_IS_BUSY)
            {
                ((Command)SignInCommand).ChangeCanExecute();
                ((Command)ForgotCommand).ChangeCanExecute();
                ((Command)SignUpCommand).ChangeCanExecute();
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
                    this.OnSubmitExecute();
                    //do you task  
                }
                else
                {
                    Message = ResponceCode.customErrorFunction(651, null);
                }

            }
        }

        public async void OnSubmitExecute()
        {
            //#if OFFLINE_SYNC_ENABLED
            try
            {
                AccountService userService = new AccountService();
                LoginModel loginModel = new LoginModel();
                loginModel.Email = _userName.Value;
                loginModel.Password = Password.Value;
                TokenModel tokenModel = await userService.AuthorizeUser(loginModel);
                if (tokenModel != null)
                {
                    if (tokenModel.message != null || string.IsNullOrEmpty(tokenModel.userId))
                    {
                        //  Client.IsLogged = false;
                        Message = tokenModel.message;
                    }
                    else
                    {
                        Client.IsUserMatched = true;
                        Client.IsRoleMatched = true;
                       
                        if (tokenModel.username != Client.username && !string.IsNullOrEmpty(Client.username))
                        Client.IsUserMatched = false;

                        Client.IsLogged = true;
                        Client.IsPermission = true;
                        Client.token = tokenModel.token;
                        Client.username = tokenModel.username;
                        Client.userId = tokenModel.userId;

                        Client.PendingCount = 0;
                        RoleService role = new RoleService();
                        ObservableCollection<RoleModel> RoleList = new ObservableCollection<RoleModel>();
                        RoleList = await role.GetUserRole();                       
                        if (RoleList.Count == 1)
                        {
                            foreach (var item in RoleList)
                            {
                                await AccountService.UpdatePreferredRole(item.Id);
                                if (item.RoleName != Client.roleName && !string.IsNullOrEmpty(Client.roleName))
                                    Client.IsRoleMatched = false;
                                Client.roleId = item.Id;
                                Client.roleName = item.RoleName;
                            }
                            if (Client.IsUserMatched == false || Client.IsRoleMatched == false)
                                 ClearedDataSync();
                            SetApplicationData(true);
                            //await System.Threading.Tasks.Task.Run(() =>
                            // {
                                 DataSync();
                           //}).ConfigureAwait(false);
                       
                            await Application.Current.MainPage.Navigation.PushAsync(new IncidentOverviewPage());
                        }
                        else if (RoleList.Count > 1)
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new RolePage(RoleList));
                        }
                        else
                        {
                            Message = ResponceCode.customErrorFunction(651, null);
                        }

                    }
                }
                else
                {
                    Message = tokenModel.message;
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
            //#else
            //              Message = "Please check Network connection.";
            //#endif

        }

        private async void ClearedDataSync()
        {
            await ClearedDataAsync();
        }

        private static async void DataSync()
        {
          //  await AllSyncService.DataAsync();
            await AllSyncService.DataAsync(true);
        }

        public override void Validate()
        {
            base.Validate();
            _userName.Validate();
            _password.Validate();
            this.SetValid();
        }

        public override void SetValid()
        {
            IsValid = _userName.IsValid && _password.IsValid;
        }

        private void AddValidations()
        {
            _userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("EMAIL_REQUIRED") });
            _userName.Validations.Add(new EmailValidator<string> { ValidationMessage = ResponceCode.applicationErrorHandler("EMAIL_VALID") });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = ResponceCode.applicationErrorHandler("PASSWORD_REQUIRED") });
        }


        public async Task OnForgot()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ForgotPassword());
        }
        public async Task OnSignUp()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SignUpPage());
        }
    }
}
