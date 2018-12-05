using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.Services.Tables;
using LGSEApp.Validations;
using LGSEApp.View;
using LGSEApp.ViewModels.Base;
using Microsoft.WindowsAzure.MobileServices;
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
    public class SummaryViewModel : ViewModelBase
    {
        PropertyUserStatusService manager;
        PropertiesService propertyService;

        public ICommand CancelCommand { get; private set; }
        public ICommand SubmitCommand { get; private set; }
        private bool _Isnotes;
        public bool Isnotes
        {
            get { return _Isnotes; }
            set
            {
                _Isnotes = value;
                RaisePropertyChanged(() => Isnotes);
            }
        }
        private bool _IsSubStatus;
        public bool IsSubStatus
        {
            get { return _IsSubStatus; }
            set
            {
                _IsSubStatus = value;
                RaisePropertyChanged(() => IsSubStatus);
            }
        }
        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                RaisePropertyChanged(() => Notes);
            }
        }
        public PropertyDetailViewModel _propertyModel { get; set; }
        public PropertyDetailViewModel PropertyModel
        {
            get { return _propertyModel; }
            set
            {
                _propertyModel = value;
                RaisePropertyChanged(() => PropertyModel);
            }
        }
        public SummaryViewModel(PropertyDetailViewModel viewModel)
        {
            manager = PropertyUserStatusService.DefaultManager;
            propertyService = PropertiesService.DefaultManager;
            SubmitCommand = new Command(() => OnSubmit(), () => !IsBusy);
            CancelCommand = new Command(async () => await OnCancel(), () => !IsBusy);
            PropertyModel = viewModel;
            IsSubStatus = (PropertyModel.SubStatus == null ? false : true);
            this.PropertyChanged += SummaryViewModel_PropertyChanged;
        }

        private void SummaryViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PROP_NAME_IS_BUSY)
            {
                ((Command)SubmitCommand).ChangeCanExecute();
                ((Command)CancelCommand).ChangeCanExecute();

            }
        }

        public override void Validate()
        {
            base.Validate();
            //if (string.IsNullOrEmpty(_notes))
            //{
            //    IsValid = false;
            //    Isnotes = true;
            //}
            //else
            //{
            //    IsValid = true;
            //    Isnotes = false;
            //}
            this.SetValid();
        }

        public override void SetValid()
        {
            IsValid = IsValid;
        }
        void OnSubmit()
        {
            //Validate();
            //if (IsValid)
            //{
            IsBusy = true;
            this.OnSubmitExcute();
            //   }
        }
       async  void OnSubmitExcute()
        {
            try
            {
               await  PostPropertyUserStatus();
              await   UpdatePropertyStatus();
                //Check Permissions 401 && 403
               // IsCheckPermissions();
                await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
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

        private async Task PostPropertyUserStatus()
        {
            try
            {
                PropertyUserStatus propertyUserStatus = new PropertyUserStatus();
              //  propertyUserStatus.Id = PropertyModel.PropertyModel.Id;
                propertyUserStatus.UserId = Client.userId;
                propertyUserStatus.PropertyId = PropertyModel.PropertyModel.PropertyId;
                propertyUserStatus.StatusId = PropertyModel.Status.Id;
                propertyUserStatus.PropertySubStatusMstrsId = (PropertyModel.SubStatus == null ? null : PropertyModel.SubStatus.Id);
                propertyUserStatus.Notes = Notes;
                propertyUserStatus.PropertyUserMapsId = PropertyModel.PropertyModel.Id;
                propertyUserStatus.UserId = Client.userId;
                propertyUserStatus.RoleId = Client.roleId;
                propertyUserStatus.StatusChangedOn = DateTime.UtcNow;
                propertyUserStatus.CreatedBy = Client.userId;
                await manager.SaveTaskAsync(propertyUserStatus);
                networkAccess = Connectivity.NetworkAccess;
                if (networkAccess == NetworkAccess.Internet)
                {

                    await manager.SyncAsync();
                }
                else
                {
                    Client.PendingCount = Client.PendingCount + 1;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
        private async Task UpdatePropertyStatus()
        {
            try
            {

                Property property = new Property();
                property.Id = PropertyModel.PropertyModel.Id;
                property.PropertyId = PropertyModel.PropertyModel.PropertyId;
                property.MPRN = PropertyModel.PropertyModel.MPRN;
                property.BuildingName = PropertyModel.PropertyModel.BuildingName;
                property.SubBuildingName = PropertyModel.PropertyModel.SubBuildingName;
                property.MCBuildingName = PropertyModel.PropertyModel.MCBuildingName;
                property.MCSubBuildingName = PropertyModel.PropertyModel.MCSubBuildingName;
                property.BuildingNumber = PropertyModel.PropertyModel.BuildingNumber;
                property.PrincipalStreet = PropertyModel.PropertyModel.PrincipalStreet;
                property.DependentStreet = PropertyModel.PropertyModel.DependentStreet;
                property.PostTown = PropertyModel.PropertyModel.PostTown;
                property.LocalityName = PropertyModel.PropertyModel.LocalityName;
                property.DependentLocality = PropertyModel.PropertyModel.DependentLocality;
                property.Country = PropertyModel.PropertyModel.Country;
                property.Postcode = PropertyModel.PropertyModel.Postcode;
                property.PriorityCustomer = PropertyModel.PropertyModel.PriorityCustomer;
                property.Zone = PropertyModel.PropertyModel.Zone;
                property.Cell = PropertyModel.PropertyModel.Cell;
                property.IncidentId = PropertyModel.PropertyModel.IncidentId;
                property.IncidentName = PropertyModel.PropertyModel.IncidentName;
                property.CellManagerId = PropertyModel.PropertyModel.CellManagerId;
                property.ZoneManagerId = PropertyModel.PropertyModel.ZoneManagerId;
                property.Status = PropertyModel.PropertyModel.Status;
                property.LatestStatus = PropertyModel.Status.Status;
                property.LatestSubStatus = (PropertyModel.SubStatus == null ? null : PropertyModel.SubStatus.SubStatus);
                property.Notes = Notes;
                property.Latitude = PropertyModel.PropertyModel.Latitude;
                property.Longitude = PropertyModel.PropertyModel.Longitude;
                property.IsMPRNAssigned = PropertyModel.PropertyModel.IsMPRNAssigned;
                property.AssignedResourceCount = PropertyModel.PropertyModel.AssignedResourceCount;
                property.IsStatusUpdated = true;
                property.IsUnassigned = false;
                property.StatusChangedOn = DateTime.UtcNow;
                property.Deleted = PropertyModel.PropertyModel.Deleted;
                property.IsLastStatusUpdate = true;
                property.UpdatedAt = PropertyModel.PropertyModel.UpdatedAt;
                property.CreatedAt = PropertyModel.PropertyModel.CreatedAt;
                property.CreatedBy = PropertyModel.PropertyModel.CreatedBy;
                property.Version = PropertyModel.PropertyModel.Version;
                await propertyService.SaveTaskAsync(property);
                //if (networkAccess == NetworkAccess.Internet)
                //{

                //    await propertyService.SyncAsync();
                   
                //}
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
        async Task OnCancel()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }

    }
}
