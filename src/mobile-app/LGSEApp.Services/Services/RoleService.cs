using LGSEApp.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LGSEApp.Services.Services
{
    public class RoleService 
    {
        static string SignUpRoleurl = "api/Role";
        static string UserRoleurl = "api/Account/UserProfile";
        public async Task<List<RoleModel>> GetSignUpRole()
        {
            List<RoleModel> roleModel = null;

            try
            {

                HttpResponseMessage httpRespon = await Client.GetAsAsync(SignUpRoleurl);
                if (httpRespon.IsSuccessStatusCode)
                {
                    var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                    return roleModel = JsonConvert.DeserializeObject<List<RoleModel>>(responJsonText);
                }

            }
            catch (Exception Ex)
            {

                Debug.Write(Ex.Message);
            }
            return roleModel;
        }
        public async Task<ObservableCollection<RoleModel>> GetUserRole()
        {
            ObservableCollection<RoleModel> roleModel = new ObservableCollection<RoleModel>();
            UserProfileModel userModel = new UserProfileModel();
            try
            {
                //Client client = new Client();
                HttpResponseMessage httpRespon = await Client.GetAsAsync(UserRoleurl);

                if (httpRespon.IsSuccessStatusCode)
                {
                    var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                    userModel = JsonConvert.DeserializeObject<UserProfileModel>(responJsonText);
                    Client.firstName = userModel.FirtName;
                    Client.lastName = userModel.LastName;
                    foreach (var item in userModel.roles)
                    {
                        roleModel.Add(new RoleModel() { Id = item.Id, RoleName = item.RoleName });
                    }
                }
              
            }

            
            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);

            }
            return roleModel;
        }
    }
}
