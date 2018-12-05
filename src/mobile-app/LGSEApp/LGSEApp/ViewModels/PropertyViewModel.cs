using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LGSEApp.ViewModels
{
    public class PropertyViewModel : ViewModelBase
    {
        PropertiesService manager;
        private const string BackColor = "#FFFFFF";
        public ICommand FilterCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public Command LoadItemsCommand { get; set; }
        public ICommand SearchCommand { get; private set; }
        private ObservableCollection<PropertyModel> items { get; set; }
        public ObservableCollection<PropertyModel> Items
        {
            get { return items; }
            set
            {
                items = value;
                RaisePropertyChanged(() => Items);
            }
        }
        private ObservableCollection<SortingModel> sortingList { get; set; }
        public ObservableCollection<SortingModel> SortingList
        {
            get { return sortingList; }
            set
            {
                sortingList = value;
                RaisePropertyChanged(() => SortingList);
            }
        }
        private SortingModel _sort;
        public SortingModel Sort
        {
            get
            {
                return _sort;
            }
            set
            {
                _sort = value;

                RaisePropertyChanged(() => Sort);

            }
        }
        private string isBlur;
        public string IsBlur
        {
            get { return isBlur; }
            set
            {
                isBlur = value;
                RaisePropertyChanged(() => IsBlur);
            }
        }
        private bool _IsInProgress;
        public bool IsInProgress
        {
            get { return _IsInProgress; }
            set
            {
                _IsInProgress = value;
                RaisePropertyChanged(() => IsInProgress);
                if (!IsInProgress)
                {
                    IsStatusText = "In Progress";
                }
                else
                {
                    IsStatusText = "Completed";
                }
            }
        }
        private string _IsStatusText;
        public string IsStatusText
        {
            get { return _IsStatusText; }
            set
            {
                _IsStatusText = value;
                RaisePropertyChanged(() => IsStatusText);


            }
        }
        private bool _IsPriority;
        public bool IsPriority
        {
            get { return _IsPriority; }
            set
            {
                _IsPriority = value;
                RaisePropertyChanged(() => IsPriority);
                if (!IsPriority)
                {
                    IsPriorityText = "All Customer";
                }
                else
                {
                    IsPriorityText = "Priority Customer";
                }
            }
        }
        private string _IsPriorityText;
        public string IsPriorityText
        {
            get { return _IsPriorityText; }
            set
            {
                _IsPriorityText = value;
                RaisePropertyChanged(() => IsPriorityText);


            }
        }
        private bool _IsListEnabled;
        public bool IsListEnabled
        {
            get { return _IsListEnabled; }
            set
            {
                _IsListEnabled = value;
                RaisePropertyChanged(() => IsListEnabled);
            }
        }
        private bool _IsFilter;
        public bool IsFilter
        {
            get { return _IsFilter; }
            set
            {
                _IsFilter = value;
                RaisePropertyChanged(() => IsFilter);
            }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    RaisePropertyChanged(() => SearchText);
                    if (SearchCommand.CanExecute(null))
                    {
                        SearchCommand.Execute(null);
                    }
                }

            }
        }
        public PropertyViewModel(bool sync)
        {          
           
            manager = PropertiesService.DefaultManager;
            IsListEnabled = true;
            Items = new ObservableCollection<PropertyModel>();
            SortingList = new ObservableCollection<SortingModel>();
            FilterCommand = new Command(() => OnLoadFilter());
            ApplyCommand = new Command(() => OnApply());
            ClearCommand = new Command(() => OnClear());
            SearchCommand = new Command(() => OnSearch());
            CancelCommand = new Command(() => OnCancel());
            Filter();
            LoadExcuteProperty(sync);
            LoadItemsCommand = new Command(async () =>  ExecuteLoadItemsCommand());
            IsBlur = BackColor;
        }

        private async void OnSearch()
        {
            LoadExcuteProperty();
        }

        async void Filter()
        {
            GetSortList();
            IsInProgress = false;
            IsPriority = false;
        }
        async void OnClear()
        {
            Filter();
        }
        async void GetSortList()
        {
            SortingList.Clear();
            var sortItems = await SortingData.GetSortList();
            foreach (var item in sortItems)
            {
                if (item.id == 0)
                    Sort = item;
                SortingList.Add(item);
            }

        }
        private async void OnApply()
        {
            LoadExcuteProperty();
            IsListEnabled = true;
            IsFilter = false;
        }

        private void OnCancel()
        {
            IsListEnabled = true;
            IsFilter = false;
        }

        public void OnLoadFilter()
        {
            IsFilter = true;
            IsListEnabled = false;
        }
        async void LoadExcuteProperty(bool syncItems = false)
        {
           await  ExecuteLoadItemsCommand(syncItems);
        }

        async Task ExecuteLoadItemsCommand(bool syncItems = true)
        {
            if (IsBusy)
            {
                IsBusy = false;
                return;
            }

            IsBusy = true;
            try
            {
                Items.Clear();
                networkAccess = Connectivity.NetworkAccess;
                syncItems = (networkAccess == NetworkAccess.Internet && syncItems ? true : false);
                if (_sort == null)
                    _sort = new SortingModel() { id = 0, SortingText = "PostCode Asc" };
                var items = await manager.GetPropertyItemsAsync(_searchText, _IsInProgress, _IsPriority, _sort.id, syncItems);
                //Check Permissions 401 && 403
                IsCheckPermissions();
                foreach (var item in items)
                {
                    Items.Add(new PropertyModel() { FirstRow = item.BuildingNumber + (item.BuildingNumber.Length == 0 ? "" : (item.BuildingName.Length == 0 ? "" : ", ")) + item.BuildingName, SecoundRow = item.PrincipalStreet + (item.PrincipalStreet.Length == 0 ? "" : (Convert.ToString(item.Postcode).Length == 0 ? "" : ", ")) + item.Postcode, Id = item.Id, Postcode = item.Postcode, BuildingName = item.BuildingName, BuildingNumber = item.BuildingNumber, Cell = item.Cell, CellManagerId = item.CellManagerId, Country = item.Country, DependentLocality = item.DependentLocality, DependentStreet = item.DependentStreet, IncidentId = item.IncidentId, LocalityName = item.LocalityName, MCBuildingName = item.MCBuildingName, MCSubBuildingName = item.MCSubBuildingName, MPRN = item.MPRN, ConcatStatus = (string.IsNullOrEmpty(item.LatestStatus) ? "" : item.LatestStatus + (string.IsNullOrEmpty(item.LatestSubStatus) ? "" : ", " + item.LatestSubStatus)), Status = item.Status, PostTown = item.PostTown, PrincipalStreet = item.PrincipalStreet, PriorityCustomer = item.PriorityCustomer, SubBuildingName = item.SubBuildingName, Zone = item.Zone, ZoneManagerId = item.ZoneManagerId, Latitude = item.Latitude, Longitude = item.Longitude, LatestStatus = item.LatestStatus, LatestSubStatus = item.LatestSubStatus, AssignedResourceCount = item.AssignedResourceCount, IncidentName = item.IncidentName, IsStatusUpdated = item.IsStatusUpdated, Notes = (string.IsNullOrEmpty(item.Notes) ? "" : item.Notes), StatusChangedOn = item.StatusChangedOn, PropertyId = item.PropertyId, CreatedAt = item.CreatedAt, CreatedBy = item.CreatedBy, Deleted = item.Deleted, IsIsolated = item.IsIsolated, IsLastStatusUpdate = item.IsLastStatusUpdate, IsMPRNAssigned = item.IsMPRNAssigned, IsUnassigned = item.IsUnassigned, ModifiedBy = item.ModifiedBy, NotesCount = item.NotesCount, UpdatedAt = item.UpdatedAt, Version = item.Version });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        
    }
}
