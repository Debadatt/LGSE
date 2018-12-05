using LGSEApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LGSEApp.Services;
using System.Threading.Tasks;
using System.Linq;
[assembly: Xamarin.Forms.Dependency(typeof(LGSEApp.Services.PropertyDataStore))]
namespace LGSEApp.Services
{
    
    public class PropertyDataStore : IDataStore<PropertyModel>
    {
        List<PropertyModel> items;

        public PropertyDataStore()
        {
            items = new List<PropertyModel>();
            var mockItems = new List<PropertyModel>
            {
               new PropertyModel { Id = "7226468C-FDA7-4C3C-9F75-AD83DF9CE570", IncidentId = "INC-001", BuildingName="ABC", Postcode="00001",PriorityCustomer=true,PrincipalStreet="1"},
             new PropertyModel { Id = "AC4D931E-B40E-4EA9-9C29-06540950B28C", IncidentId = "INC-001", BuildingName="XYZ", Postcode="00001",PriorityCustomer=true,PrincipalStreet="1"},
             new PropertyModel { Id = "F508ED3F-434F-40E6-8D41-18FB98881E46", IncidentId = "INC-001", BuildingName="DFS", Postcode="00001",PriorityCustomer=false,PrincipalStreet="1"},
             new PropertyModel { Id = "D635B802-4DDA-4EE2-8814-F35D07091875", IncidentId = "INC-001", BuildingName="North", Postcode="00001",PriorityCustomer=true,PrincipalStreet="1"},
             new PropertyModel { Id = "F9902F3E-9432-41FF-9403-CAFE5BDF4DFA", IncidentId = "INC-001", BuildingName="West", Postcode="00001",PriorityCustomer=false,PrincipalStreet="1"},
             new PropertyModel { Id = "", IncidentId = "INC-001", BuildingName="South", Postcode="00001",PriorityCustomer=true,PrincipalStreet="1"},

            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(PropertyModel item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(PropertyModel item)
        {
            var _item = items.Where((PropertyModel arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(PropertyModel item)
        {
            var _item = items.Where((PropertyModel arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<PropertyModel> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<PropertyModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
