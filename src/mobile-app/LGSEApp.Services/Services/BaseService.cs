/*
 * To add Offline Sync Support:
 *  1) Add the NuGet package Microsoft.Azure.Mobile.Client.SQLiteStore (and dependencies) to all client projects
 *  2) Uncomment the #define OFFLINE_SYNC_ENABLED
 *
 * For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342
 */
#define OFFLINE_SYNC_ENABLED

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LGSEApp.Services.Tables;
using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace LGSEApp.Services.Services
{
    public class BaseService
    {
       static public MobileServiceClient client;
        
#if OFFLINE_SYNC_ENABLED
      
   static     public IMobileServiceSyncTable<Property> propertyTable;
        static public IMobileServiceSyncTable<PropertyStatusMstr> propertyStatusMstrTable;
   static     public IMobileServiceSyncTable<PropertySubStatusMstr> propertySubStatusMstrTable;
        static public IMobileServiceSyncTable<PropertyUserStatus> propertyUserStatusTable;
#else
 static  public     IMobileServiceTable<Properties> propertyTable;        
 static  public     IMobileServiceTable<PropertyStatusMstr> propertyStatusMstrTable;        
static public     IMobileServiceTable<PropertySubStatusMstr> propertySubStatusMstrTable; 
  static      public     IMobileServiceTable<PropertyUserStatus> propertyUserStatusTable; 
#endif

        public const string offlineDbPath = @"localstore.db";
     
        static BaseService()
        {
            //HttpRequestMessage request=new HttpRequestMessage();
            //request.Headers.Add("X-ZUMO-AUTH", Client.token);
            //request.RequestUri = new Uri(Constants.ApplicationURL);
            //HttpMessageHandler messageHandler;
            
            client = new MobileServiceClient(Constants.app_table_api,new MyHandler());
            // this.client = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            // var store = new MobileServiceSQLiteStore(offlineDbPath);
        var  store = new MobileServiceSQLiteStore(offlineDbPath);
        store.DefineTable<Property>();
            store.DefineTable<PropertyStatusMstr>();
            store.DefineTable<PropertySubStatusMstr>();
            store.DefineTable<PropertyUserStatus>();
            
            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            client.SyncContext.InitializeAsync(store);
         
            propertyTable = client.GetSyncTable<Property>();
            propertyStatusMstrTable = client.GetSyncTable<PropertyStatusMstr>();
            propertySubStatusMstrTable = client.GetSyncTable<PropertySubStatusMstr>();
          propertyUserStatusTable = client.GetSyncTable<PropertyUserStatus>();
            
#else
            propertyTable = client.GetTable<Property>();
            propertyStatusMstrTable = client.GetTable<PropertyStatusMstr>();
            propertySubStatusMstrTable = client.GetTable<PropertySubStatusMstr>();
            propertyUserStatusTable = client.GetTable<PropertyUserStatus>();
            
          
#endif
        }
        public static async void DataPurshingAsync()
        {
            if (client.SyncContext.PendingOperations == 0)
                await client.SyncContext.PushAsync();
           // Remove the token from the MobileServiceClient
           await client.LogoutAsync();
        }
    }
    public class MyHandler : DelegatingHandler
    {
        protected  override  async Task<HttpResponseMessage>
            SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {

                // Change the request-side here based on the HttpRequestMessage
                request.Headers.Clear();
                request.Headers.Add("ZUMO-API-VERSION", "2.0.0");
                request.Headers.Add("X-ZUMO-AUTH", Client.token);
                request.Headers.Add("X-ACCESS-ROLE", Client.roleId);
                var request1 = request.RequestUri.ToString().Replace("updatedAt", "UpdatedAt");
                request.RequestUri = new Uri(request1);
                Debug.WriteLine(request1);
                Debug.WriteLine(Client.token);
                Debug.WriteLine(Client.roleId);
                // Do the request

                response = await base.SendAsync(request, cancellationToken);
                if (Convert.ToInt32(response.StatusCode) == 401)
                {
                    Debug.WriteLine(ResponceCode.customErrorFunction(401, null));
                    Client.IsLogged = false;
                    return null;
                }
                else if(Convert.ToInt32(response.StatusCode) == 403)
                {
                    Debug.WriteLine(ResponceCode.customErrorFunction(403, null));
                    Client.IsPermission = false;
                    return null;
                }
               
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
            return response;
        }
    }
}
