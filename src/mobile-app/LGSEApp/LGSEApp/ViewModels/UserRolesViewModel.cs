using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.View;
using LGSEApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LGSEApp.ViewModels
{
    public class UserRolesViewModel : ViewModelBase
    {


        public ObservableCollection<RoleModel> RoleList { get; set; }
        private RoleModel _Roles;
        public RoleModel Roles
        {
            get
            {
                return _Roles;
            }
            set
            {
                _Roles = value;
                RaisePropertyChanged(() => Roles);

            }
        }



        public ICommand SubmitCommand { get; private set; }
        public UserRolesViewModel()
        {
            SubmitCommand = new Command(() => OnSubmit(), () => !IsBusy);
            RoleList = new ObservableCollection<RoleModel>();
            this.PropertyChanged += SubmitCommand_PropertyChanged;
        }
        private void SubmitCommand_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PROP_NAME_IS_BUSY)
            {
                ((Command)SubmitCommand).ChangeCanExecute();
               
            }
        }
        public  void OnSubmit()
        {
            Validate();
            if (IsValid)
            {
                networkAccess = Connectivity.NetworkAccess;
                if (networkAccess == NetworkAccess.Internet)
                {
                    IsBusy = true;
                     this.OnSubmitExcuteAsync();
                }
                else
                {
                    Message = ResponceCode.customErrorFunction(651, null);
                }
            }
           
        }

        private async void OnSubmitExcuteAsync()
        {
            try
            {

                if (Client.roleName != _Roles.RoleName && !string.IsNullOrEmpty(Client.roleName))
                    Client.IsRoleMatched = false;
                if (Client.IsUserMatched == false || Client.IsRoleMatched == false)
                    await ClearedDataAsync();

                Client.roleId = _Roles.Id;
                Client.roleName = _Roles.RoleName;
                SetApplicationData(true);
                 UpdateRoleSync();
                await Application.Current.MainPage.Navigation.PushAsync(new IncidentOverviewPage());
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

        private async void UpdateRoleSync()
        {
            await AccountService.UpdatePreferredRole(_Roles.Id);
           
             // await   AllSyncService.DataAsync();
             await   AllSyncService.DataAsync(true);
          
          
        }

        public override void Validate()
        {
            base.Validate();
            if (_Roles != null)
            {
                Message = "";               
            }
            else
            {
                Message = ResponceCode.applicationErrorHandler("ROLE_REQUIRED");
            }
            this.SetValid();
        }
        public override void SetValid()
        {
            IsValid = (_Roles == null ? false : true);
        }
    }
}
