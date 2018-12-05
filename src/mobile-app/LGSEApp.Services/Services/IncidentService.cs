using LGSEApp.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LGSEApp.Services.Services
{
   public class IncidentService
    {
        static string UserRoleurl = "api/IncidentCustom/GetInprogressIncident";
        static string GetAuditurl = "api/IncidentCustom/MPRNStatusHistory?propertyId=";

        #region Get In Progress Incident Detail Like Incident Id and Incident Name.
        public static async Task<IncidentModel> GetInProgressIncidentDetail()
        {

            IncidentModel propertyModels = null;
            
            try
            {
               // Client client = new Client();
                HttpResponseMessage httpRespon = await Client.GetAsAsync(UserRoleurl);
                if (httpRespon.IsSuccessStatusCode)
                {
                    var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                    propertyModels = JsonConvert.DeserializeObject<IncidentModel>(responJsonText);
                    
                }

            }
            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);

            }
            return propertyModels;
        }
        #endregion
        #region Get History for Property Status.
        public static async Task<ObservableCollection<PropertyStatusHistoryModel>> GetPropertyHistory(string id)
        {

          ObservableCollection<PropertyStatusHistoryModel> propertyModels = null;

            try
            {
               
                HttpResponseMessage httpRespon = await Client.GetAsAsync(GetAuditurl+ id);
                if (httpRespon.IsSuccessStatusCode)
                {
                    var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                    propertyModels = JsonConvert.DeserializeObject<ObservableCollection<PropertyStatusHistoryModel>>(responJsonText);

                }

            }
            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);

            }
            return propertyModels;
        }
        #endregion
    }
}
