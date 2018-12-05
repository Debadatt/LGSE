
#define OFFLINE_SYNC_ENABLED
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace LGSEApp.Services.Services
{
    public static class AllSyncService
    {
        static PropertiesService manager;
        static PropertyUserStatusService propertyUserStatusService;
        static PropertyStatusMstrService propertyStatus;
        static PropertySubStatusMstrService propertySub;
        static AllSyncService()
        {
            manager = PropertiesService.DefaultManager;
            propertyUserStatusService = PropertyUserStatusService.DefaultManager;
            propertyStatus = PropertyStatusMstrService.DefaultManager;
            propertySub = PropertySubStatusMstrService.DefaultManager;
        }

        #region All Data Sync in local Database.
        public static async Task DataAsync(bool IsSync = false)
        {
#if OFFLINE_SYNC_ENABLED
            try
            {

              await  IncidentDataDelete();
                await propertyStatus.SyncAsync();
                await propertySub.SyncAsync();
                await propertyUserStatusService.SyncAsync();
                await manager.SyncAsync(IsSync);
               
               
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
#endif
        }
        #endregion

        #region All Data Push on Server.
        public static async Task PushDataAsync()
        {
#if OFFLINE_SYNC_ENABLED
            try
            {
                await propertyUserStatusService.SyncAsync();

            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
#endif
        }
        #endregion
        #region
        public static async Task IncidentDataDelete()
        {
            try
            {
                var incidentDetail = await IncidentService.GetInProgressIncidentDetail();
                if (incidentDetail != null)
                {
                    if (incidentDetail.status == 1 || incidentDetail.status == 2)
                    {
                        Debug.Write(incidentDetail.id);
                        await DataIncidentAsync(incidentDetail.id,false);
                    } 
                    else
                    {
                        Debug.Write(incidentDetail.id);
                        await DataIncidentAsync(incidentDetail.id);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
        #endregion
        #region Push data on live for PropertyUserStatus Table.
        public static async void DataPushAsync(bool IsSync = false)
        {
#if OFFLINE_SYNC_ENABLED
            try
            {            
                await propertyUserStatusService.SyncAsync();
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
#endif
        }
        #endregion

        #region Delete Completed or cancel Incedent Properties in local table
        public static async Task DataIncidentAsync(string incidentId=null,bool flag=true)
        {
            try
            {
                await manager.DeleteTaskAsync(incidentId, flag); //Property Table
           
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }
        #endregion

        #region Delete All local table data.
        public static async Task OnLogOutAsync()
        {
            try
            {
                
                // var store = new MobileServiceSQLiteStore("localstore.db");
                // IMobileServiceSyncTable config =  BaseService.client.GetSyncTable("_config");
                //  var data = await config.;
                //  Debug.WriteLine(data);
                BaseService.DataPurshingAsync();
                await manager.DeleteTaskAsync(); //Property Table
                await propertyUserStatusService.DeleteTaskAsync(); //Property Status Master Table
                await propertyStatus.DeleteTaskAsync();//Property Sub Status Master Table
                await propertySub.DeleteTaskAsync();//Property User Status Table

            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
        #endregion
    }
}
