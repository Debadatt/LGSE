using LGSEApp.Services.Models;
using LGSEApp.Services.Services;
using LGSEApp.View;
using LGSEApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LGSEApp.ViewModels
{
    public class PropertyHistoryViewModel : ViewModelBase
    {
        private string mprn { get; set; }
        public string MPRN
        {
            get { return mprn; }
            set
            {
                mprn = value;
                RaisePropertyChanged(() => MPRN);
            }
        }
        private ObservableCollection<PropertyHistoryModel> propertyHistoryModels { get; set; }
        public ObservableCollection<PropertyHistoryModel> PropertyHistoryModels
        {
            get { return propertyHistoryModels; }
            set
            {
                propertyHistoryModels = value;
                RaisePropertyChanged(() => PropertyHistoryModels);
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
        public PropertyHistoryViewModel(PropertyModel item)
        {
            PropertyModel = item;
            MPRN = string.Concat("MPRN: ",item.MPRN);
            PropertyHistoryModels = new ObservableCollection<PropertyHistoryModel>();
            LoadExcuteProperty(item.PropertyId);
        }

        private async void LoadExcuteProperty(string id)
        {
            await LoadPropertyHistory(id);
        }
      
        private async Task LoadPropertyHistory(string id)
        {
            try
            {                               
                    PropertyHistoryModels.Clear();
                    var items = await IncidentService.GetPropertyHistory(id);
                    foreach (var item in items)
                    {
                        PropertyHistoryModels.Add(new PropertyHistoryModel() { FirstRow = string.Concat(item.firstName, " ", item.lastName) + (item.roleName.Length == 0 ? "" : ", "+ item.roleName), SecoundRow = item.status +  (string.IsNullOrEmpty(item.subStatus)? "" : ", "+ item.subStatus) , Notes = (string.IsNullOrEmpty(item.notes) ? "" : item.notes), StatusChangedOn = item.statusChangedOn });
                    }
              
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
    }
}
