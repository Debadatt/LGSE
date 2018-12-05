using LGSEApp.Services.Services;
using LGSEApp.View;
using LGSEApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGSEApp.ViewModels
{
    public class IncidentOverviewViewModel : ViewModelBase
    {

        public ICommand SubmitCommand { get; private set; }
      
        private string _incidentDetails;
        public string IncidentDetails
        {
            get
            {
                return _incidentDetails;
            }
            set
            {
                _incidentDetails = value;
                RaisePropertyChanged(() => IncidentDetails);
            }
        }
        public IncidentOverviewViewModel()
        {           
            ExecuteLoadItemsCommand();
            SubmitCommand = new Command(async () => await OnSubmit());
        }
     async   void ExecuteLoadItemsCommand()
        {

            try
            {
             //   IsBusy = true;
                IncidentDetails = string.Empty;
                var data = await AccountService.GetIncidentOverview();
                if(data!=null )
                IncidentDetails = (string.IsNullOrEmpty(data.Description) ? "" : data.Description);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
               // IsBusy = false;
            }
        }
        public async Task OnSubmit()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new MainPage(true));
        }

    }
}
