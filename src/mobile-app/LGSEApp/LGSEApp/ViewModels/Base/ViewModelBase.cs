using LGSEApp.Services;
using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LGSEApp.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        // protected readonly IDialogService DialogService;
        //  protected readonly INavigationService NavigationService;
        public ICommand SettingCommand { get; private set; }
        public const string PROP_NAME_IS_BUSY = "IsBusy";
        private NetworkAccess _networkAccess;
        public NetworkAccess networkAccess
        {
            get
            {
                return _networkAccess;
            }

            set
            {
                _networkAccess = value;
                RaisePropertyChanged(() => networkAccess);
            }
        }


        private bool _isLogged;
        public bool IsLogged
        {
            get
            {
                return _isLogged;
            }

            set
            {
                _isLogged = value;
                RaisePropertyChanged(() => IsLogged);
            }
        }
        private bool _isBusy;

        public bool CanValidate { get; set; }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }
        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;

                RaisePropertyChanged(() => Message);
            }
        }

      Page page=new Page();

        public async void IsCheckPermissions()
        {
            if (Client.IsLogged == false)
                await LogOut();
            else if (!Client.IsPermission)
                await page.DisplayAlert("Access Permission", ResponceCode.customErrorFunction(403, null), "Ok");
        }

        public ViewModelBase()
        {
           
            SettingCommand = new Command(async () => await OnSettingAsync());
            // DialogService = ViewModelLocator.Resolve<IDialogService>();
            //    NavigationService = ViewModelLocator.Resolve<INavigationService>();
        }

        private async Task OnSettingAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SettingPopupPage());
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        public bool isValid;

        private bool _isValid;

        public bool IsValid
        {
            get
            {
                return _isValid;
            }

            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    RaisePropertyChanged(() => IsValid);
                }
            }
        }

        public virtual void SetValid()
        {

        }

        public virtual void Validate()
        {
            this.CanValidate = true;
        }
        public void SetApplicationData(bool IsUserLoggedIn=false)
        {
            
            Application.Current.Properties["IsUserLoggedIn"] = IsUserLoggedIn;
            Application.Current.Properties["token"] = Client.token;
            Application.Current.Properties["username"] = Client.username;
            Application.Current.Properties["userId"] = Client.userId;
            Application.Current.Properties["firstName"] = Client.firstName;
            Application.Current.Properties["lastName"] = Client.lastName;
            Application.Current.Properties["roleId"] = Client.roleId;
            Application.Current.Properties["roleName"] = Client.roleName;
        }
        public void GetApplicationData()
        {
            Client.IsLogged = true;
            Client.IsPermission = true;
            Client.token = Convert.ToString(Application.Current.Properties["token"]);
            Client.username = Convert.ToString(Application.Current.Properties["username"]);
            Client.userId = Convert.ToString(Application.Current.Properties["userId"]);
            Client.firstName = Convert.ToString(Application.Current.Properties["firstName"]);
            Client.lastName = Convert.ToString(Application.Current.Properties["lastName"]);
            Client.roleId = Convert.ToString(Application.Current.Properties["roleId"]);
            Client.roleName = Convert.ToString(Application.Current.Properties["roleName"]);
        }
        public async Task ClearedDataAsync()
        {
            Client.IsCleared = true;
            await AllSyncService.OnLogOutAsync();
        }

        public async void LogOutSync()
        {
         await   LogOut();
        }
        public async Task LogOut()
        {
            try
            {
                AllSyncService.DataPushAsync();
                Client.IsLogged = false;
                Application.Current.Properties["IsUserLoggedIn"] = false;
                await AccountService.Logout();
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
    }
}
