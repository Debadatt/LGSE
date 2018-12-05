using LGSEApp.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LGSEApp.Services.Services
{
    public class StatusService
    {
        static string Statusurl = "api/PropertyStatusMstr";
        static string SubStatusurl = "api/PropertySubStatusMstr?$filter=PropertyStatusMstrsId eq ";
        static string PostStatusurl = "tables/PropertyUserStatus";

        public async Task<List<Status>> GetStatusList()
        {
            // List<Status> statusList = new List<Status>();
            List<Status> status = new List<Status>();

            try
            {
               // Client client = new Client();
                
                HttpResponseMessage httpRespon = await Client.GetAsAsync(Statusurl);
                if (httpRespon.IsSuccessStatusCode)
                {
                    var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                    status = JsonConvert.DeserializeObject<List<Status>>(responJsonText);
                    //  roleModel = userModel.roles;
                    //foreach (var item in statusList)
                    //{
                    //    status.Add(new Status() { id = item.id, status = item.status });
                    //}
                }
                //  roleModel.Add(new RoleModel() { Id = userModel.roles, RoleName = userModel.RoleId.RoleName });
            }


            catch (Exception Ex)
            {


            }
            return status;
        }
        public async Task<List<SubStatus>> GetSubStatusList( string id)
        {

            List<SubStatus> substatus = new List<SubStatus>();

            try
            {
               // Client client = new Client();
                string Getsubstatususrl = string.Format(SubStatusurl + "'{0}'", id);
                HttpResponseMessage httpRespon = await Client.GetAsAsync(Getsubstatususrl);
                if (httpRespon.IsSuccessStatusCode)
                {
                    var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                    substatus = JsonConvert.DeserializeObject<List<SubStatus>>(responJsonText);
                }

            }


            catch (Exception Ex)
            {


            }
            return substatus;
        }
        //public async Task<CustomeMessage> PostPropertyStatus(PropertyModel propertyModel)
        //{
        //    CustomeMessage message = new CustomeMessage();
        //    try
        //    {
        //        dynamic dynamicJson = new ExpandoObject();
        //        dynamicJson.PropertyId = propertyModel.id;
        //        dynamicJson.StatusId = propertyModel.status;           
        //        dynamicJson.PropertySubStatusMstrsId = propertyModel.substatus;
        //        dynamicJson.Notes = propertyModel.Notes;
        //        HttpResponseMessage httpRespon = await Client.PostAsAsync(PostStatusurl, dynamicJson);
        //        if (httpRespon.IsSuccessStatusCode)
        //        {
        //            var responJsonText = await httpRespon.Content.ReadAsStringAsync();
        //            var responce = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
        //            message.message = ResponceCode.customErrorFunction(Convert.ToInt32(httpRespon.StatusCode), httpRespon.ReasonPhrase);
        //            return message;
        //        }
        //        else
        //        {
        //            message.message = ResponceCode.customErrorFunction(Convert.ToInt32(httpRespon.StatusCode), httpRespon.ReasonPhrase);
        //        }

        //    }
        //    catch (Exception Ex)
        //    {


        //    }
        //    return message;
        //}
    }
}
