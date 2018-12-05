using LGSEApp.Services.Services;
using LGSEApp.View;
using LGSEApp.ViewModels.Base;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGSEApp.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        PropertiesService manager;
        PropertyUserStatusService propertyUserStatusService;
        PropertyStatusMstrService propertyStatus;
        PropertySubStatusMstrService propertySub;

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                RaisePropertyChanged(() => UserName);
            }
        }
        private string _RoleName;
        public string RoleName
        {
            get { return _RoleName; }
            set
            {
                _RoleName = value;
                RaisePropertyChanged(() => RoleName);
            }
        }
        private bool _isVisiable;
        public bool IsVisiable
        {
            get { return _isVisiable; }
            set
            {
                _isVisiable = value;
                RaisePropertyChanged(() => IsVisiable);
            }
        }

        public ICommand LogOutCommand { get; private set; }
        public ICommand ChangePasswordCommand { get; private set; }

        public ICommand SyncCommand { get; private set; }

        public SettingViewModel()
        {
            manager = PropertiesService.DefaultManager;
            propertyUserStatusService = PropertyUserStatusService.DefaultManager;
            propertyStatus = PropertyStatusMstrService.DefaultManager;
            propertySub = PropertySubStatusMstrService.DefaultManager;
            LogOutCommand = new Command(async () => await OnLogOutAsync());
            ChangePasswordCommand = new Command(async () => await OnChangePasswordAsync());
            UserName = string.Concat(Client.firstName, " ", Client.lastName);
            RoleName = Client.roleName;
            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    IsVisiable = true;
                    break;
                default:
                    IsVisiable = false;
                    break;
            }
            SyncCommand = new Command(async () => await OnSyncAsync());
        }

        async Task OnSyncAsync()
        {
            try
            {
                await AllSyncService.DataAsync();
                await Application.Current.MainPage.Navigation.PushAsync(new MainPage(true));
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }

        private async Task OnChangePasswordAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ChangePasswordPage());
        }

        public async Task OnLogOutAsync()
        {
           await LogOut();
        }
    }
}
