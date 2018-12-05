using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.Services.Tables;
using LGSEApp.View;
using LGSEApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LGSEApp.ViewModels
{

    public class PropertyDetailViewModel : ViewModelBase
    {
        PropertyStatusMstrService manager;
        PropertySubStatusMstrService propertySubStatusMstrService;
        private string BlurColor = "#C0808080";
        private string BackColor = "#FFFFFF";
        private string isBlur;
        private bool _isCompleted;
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                RaisePropertyChanged(() => IsCompleted);
            }
        }

        private bool _IsHistory;
        public bool IsHistory
        {
            get { return _IsHistory; }
            set
            {
                _IsHistory = value;
                RaisePropertyChanged(() => IsHistory);
            }
        }
        public string IsBlur
        {
            get { return isBlur; }
            set
            {
                isBlur = value;
                RaisePropertyChanged(() => IsBlur);
            }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }
        public ObservableCollection<PropertyStatusMstr> statusList { get; set; }
        public ObservableCollection<PropertyStatusMstr> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                RaisePropertyChanged(() => StatusList);
            }
        }
        private PropertyStatusMstr _status;
        public PropertyStatusMstr Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
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
        private bool _Isstatus;
        public bool IsStatus
        {
            get { return _Isstatus; }
            set
            {
                _Isstatus = value;
                RaisePropertyChanged(() => IsStatus);
            }
        }
        public ObservableCollection<PropertySubStatusMstr> substatusList { get; set; }

        private PropertySubStatusMstr _substatus;
        public PropertySubStatusMstr SubStatus
        {
            get { return _substatus; }
            set
            {
                _substatus = value;
                RaisePropertyChanged(() => SubStatus);
            }
        }
        public PropertyModel _propertyModel { get; set; }
        public PropertyModel PropertyModel
        {
            get { return _propertyModel; }
            set
            {
                _propertyModel = value;
                RaisePropertyChanged(() => PropertyModel);
            }
        }

        public ICommand MapCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand SubmitCommand { get; private set; }
        public ICommand HistoryCommand { get; private set; }
        public ICommand OnStatusSelected { get; private set; }
        public Command LoadSubStatusItemsCommand { get; private set; }
        public PropertyDetailViewModel(PropertyModel item)
        {
            manager = PropertyStatusMstrService.DefaultManager;
            propertySubStatusMstrService = PropertySubStatusMstrService.DefaultManager;
            statusList = new ObservableCollection<PropertyStatusMstr>();
            substatusList = new ObservableCollection<PropertySubStatusMstr>();
            SubmitCommand = new Command(() => OnSubmit(), () => !IsBusy);
            HistoryCommand = new Command(() => OnHistoryAsync(), () => !IsBusy);
            MapCommand = new Command(() => OnMap());
            CloseCommand = new Command(() => OnClose());
            _isCompleted = (item.IsStatusUpdated == false ? true : false);
            _IsHistory = (string.IsNullOrEmpty(item.LatestStatus) ? false : true);
            PropertyModel = item;
            IsBlur = BackColor;
            this.PropertyChanged += PropertyDetailViewModel_PropertyChanged;
        }

        private async void OnHistoryAsync()
        {
            IsBusy = true;
            await CallHistoryPage();
        }
        Page page = new Page();
        private async Task CallHistoryPage()
        {
            try
            {

                networkAccess = Connectivity.NetworkAccess;
                if (networkAccess == NetworkAccess.Internet)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new PropertyHistoryPage(PropertyModel));
                }
                else
                {
                    await page.DisplayAlert("Alert", ResponceCode.customErrorFunction(651, null), "Ok");
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

        private void OnClose()
        {
            IsSubStatus = false;
            IsStatus = false;
            IsBusy = false;
            IsBlur = BackColor;
        }

        private void PropertyDetailViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PROP_NAME_IS_BUSY)
            {
                ((Command)SubmitCommand).ChangeCanExecute();
                ((Command)HistoryCommand).ChangeCanExecute();
                ((Command)MapCommand).ChangeCanExecute();

            }
        }
        void OnSubmit()
        {
            IsBusy = true;
            IsBlur = BlurColor;
            ExecuteLoadItemsCommand();
        }

        async void ExecuteLoadItemsCommand()
        {
            await GetStatusAsync();
        }

        private async Task GetStatusAsync()
        {
            try
            {
                // StatusService service = new StatusService();
                statusList.Clear();
                
                var GetStatus = await manager.GetPropertyStatusItemsAsync();
             
                if (GetStatus.Count == 0)
                {
                    networkAccess = Connectivity.NetworkAccess;
                    if (networkAccess == NetworkAccess.Internet)
                    {
                        Client.IsPermission = true;
                        GetStatus = await manager.GetPropertyStatusItemsAsync(true);
                        //Check Permissions 401 && 403
                        IsCheckPermissions();
                    }
                    else
                    {
                        await page.DisplayAlert("Status Not available", ResponceCode.customErrorFunction(651, null), "Ok");
                    }
                }
                foreach (var item in GetStatus)
                {
                    statusList.Add(item);
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsStatus = (statusList.Count == 0 ? false : true);
            }
        }

        public async void ExecuteSubStatusLoadItemsCommand(PropertyDetailViewModel viewModel)
        {
            try
            {
                IsStatus = false;
                IsBlur = BlurColor;
                //  StatusService service = new StatusService();
                substatusList.Clear();
                // var temp = await service.GetSubStatusList( viewModel.Status.Id);
                var GetSubStatus =await propertySubStatusMstrService.GetPropertySubStatusItemsAsync(viewModel.Status.Id);

                //Check Permissions 401 && 403
                IsCheckPermissions();
                //if (GetSubStatus.Count == 0)
                //{
                //    networkAccess = Connectivity.NetworkAccess;
                //    if (networkAccess == NetworkAccess.Internet)
                //    {
                //        GetSubStatus =
                //            await propertySubStatusMstrService.GetPropertySubStatusItemsAsync(viewModel.Status.Id,
                //                true);

                //    }

                //}

                if (GetSubStatus.Count == 0)
                {
                    viewModel.SubStatus = null;
                    NavigateOnSummaryPage(viewModel);
                    IsSubStatus = false;
                    IsBusy = false;
                    IsBlur = BackColor;
                }
                else
                {
                    foreach (var item in GetSubStatus)
                    {
                        substatusList.Add(item);
                    }

                    IsSubStatus = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
               
            }

        }
        public void ExecuteSubStatusSelectedItemsCommand(PropertyDetailViewModel viewModel)
        {
            IsSubStatus = false;
            IsBlur = BackColor;
            NavigateOnSummaryPage(viewModel);
            IsBusy = false;
        }

        async void OnMap()
        {
            var address =  PropertyModel.Postcode;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Device.OpenUri(
                      new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode(address))));
                    break;
                case Device.Android:
                    Device.OpenUri(
                      new Uri(string.Format("geo:0,0?q={0}", WebUtility.UrlEncode(address))));
                    break;
                case Device.UWP:
                    Device.OpenUri(
                new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString(address))));//position.ToString()
                    break;
            }

            //switch (Device.RuntimePlatform)
            //{
            //    case Device.UWP:
            //        await Application.Current.MainPage.Navigation.PushAsync(new MapWebViewPage(this.PropertyModel));
            //        break;
            //    default:
            //        await Application.Current.MainPage.Navigation.PushAsync(new MapPage(this.PropertyModel));
            //        break;
            //}
        }
        async void NavigateOnSummaryPage(PropertyDetailViewModel viewModel)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SummaryPage(viewModel));
        }
    }
}
