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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LGSEApp.Services.Tables;
using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
#endif


namespace LGSEApp.Services.Services
{

    public partial class PropertiesService : BaseService
    {
        static PropertiesService defaultInstance = new PropertiesService();

        private PropertiesService()
        {
           
        }

        public static PropertiesService DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return propertyTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Property>; }
        }
        //bool isPrority, int isProgress,
        public async Task<ObservableCollection<Property>> GetPropertyItemsAsync(string value, bool inProgress, bool IsPrority, int Sorting, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED

                if (syncItems)
                {
                    try
                    {
                        
                        await AllSyncService.IncidentDataDelete();
                        await this.SyncAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }

#endif               //As suggested by Abhijeet,Rajesh is Sending the PropertyUsermapping id instead of propertyID
                IEnumerable<Property> items;

                var data = await propertyTable.ToListAsync();
                Debug.WriteLine(data.Count());

                if (IsPrority && string.IsNullOrEmpty(value))// Is Prority Customer=true and Search test is null
                {
                    //&& todoItem.Deleted==false
                    items = await propertyTable
                        .Where(todoItem => todoItem.IsStatusUpdated == inProgress && todoItem.IsUnassigned == false)
                         .Where(todoItem => todoItem.PriorityCustomer == IsPrority)
                         .ToEnumerableAsync();
                }
                else if (IsPrority && !string.IsNullOrEmpty(value))
                {
                    //&& todoItem.Deleted == false
                    items = await propertyTable
                      .Where(todoItem => todoItem.IsStatusUpdated == inProgress && todoItem.IsUnassigned == false)
                        .Where(todoItem => todoItem.PriorityCustomer == IsPrority)
                   .Where(x => x.BuildingNumber.Contains(value) || x.BuildingName.Contains(value) || x.Postcode.Contains(value) || x.PostTown.Contains(value) || x.LatestStatus.Contains(value) || x.LatestSubStatus.Contains(value))
                                            .ToEnumerableAsync();
                }
                else if (!IsPrority && !string.IsNullOrEmpty(value))
                {
                    //&& todoItem.Deleted == false
                    items = await propertyTable
                      .Where(todoItem => todoItem.IsStatusUpdated == inProgress && todoItem.IsUnassigned == false)
                   .Where(x => x.BuildingNumber.Contains(value) || x.BuildingName.Contains(value) || x.Postcode.Contains(value) || x.PostTown.Contains(value) || x.LatestStatus.Contains(value) || x.LatestSubStatus.Contains(value))

                                            .ToEnumerableAsync();
                }
                else
                {

                    items = await propertyTable
                      .Where(todoItem => todoItem.IsStatusUpdated == inProgress && todoItem.IsUnassigned == false)

                       .ToEnumerableAsync();
                }
                if (Sorting == 0)
                    items = items.OrderBy(t => t.Postcode);
                else if (Sorting == 1)
                    items = items.OrderByDescending(t => t.Postcode);
                else if (Sorting == 2)
                    items = items.OrderBy(t => t.PostTown);
                else if (Sorting == 3)
                    items = items.OrderByDescending(t => t.Postcode);

                if (inProgress)
                    items = items.Where(i => i.IsLastStatusUpdate);
                return new ObservableCollection<Property>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine("Invalid sync operation: {0}", new[] { msioe.Message });
            }
            catch (Exception e)
            {
                Debug.WriteLine("Sync error: {0}", new[] { e.Message });
            }
            return null;
        }

   

     
        public async Task SaveTaskAsync(Property item)
        {
            try
            {
               var data = await propertyTable.OrderByDescending(i => i.StatusChangedOn).ToListAsync();
                Debug.WriteLine(data.Count());
                if (item.Id == null)
                {
                    await propertyTable.InsertAsync(item);
                }
                else
                {
                    Debug.WriteLine("Panding Sync Data: {0}", client.SyncContext.PendingOperations);
                    var PropertyUserMapId = item.Id;
                  var   items = await propertyTable.Where(t => t.PropertyId == item.PropertyId).ToListAsync();
                    if (data.Count > 1)
                    {
                        foreach (var i in items)
                        {
                            item.Id = i.Id;
                            item.IsLastStatusUpdate = false;
                            
                            await propertyTable.UpdateAsync(item);
                        }

                        item.Id = PropertyUserMapId;
                        item.IsLastStatusUpdate = true;
                        await propertyTable.UpdateAsync(item);
                       
                        
                    }
                    else
                    {
                        await propertyTable.UpdateAsync(item);
                    }
                    Debug.WriteLine("Panding Sync Data: {0}", client.SyncContext.PendingOperations);
              
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }

       
        public async Task DeleteTaskAsync(string incidentId = null, bool flag=true)
        {
            try
            {

                if (string.IsNullOrEmpty(incidentId))
                {
                    await propertyTable.PurgeAsync(null, null, true, CancellationToken.None);
                }
                else if(flag)
                {
                    await propertyTable.PurgeAsync(propertyTable.Where(t => t.IncidentId != incidentId));
                }
                else
                {
                    await propertyTable.PurgeAsync(propertyTable.Where(t => t.IncidentId == incidentId));
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync(bool IsSync = false)
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                //Enable optimistic concurrency by retrieving version
           //     propertyTable.SystemProperties |= MobileServiceSystemProperties.Version;
                Debug.WriteLine("Panding Sync Data: {0}",client.SyncContext.PendingOperations);
               
                    try
                    {
                    await client.SyncContext.PushAsync();
                    }
                    catch (MobileServicePushFailedException exc)
                    {
                        if (exc.PushResult != null)
                        {
                            Debug.WriteLine(exc.PushResult.Errors);
                            syncErrors = exc.PushResult.Errors;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    if (IsSync)
                    {

                    await propertyTable.PullAsync(
                            //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                            //Use a different query name for each unique query in your program
                            null,
                            null);
                    }
                    else
                    {
                        // await  client.GetSyncTable("propertyTable").PullAsync(null,null);

                     await    propertyTable.PullAsync(
                            //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                            //Use a different query name for each unique query in your program
                            "Property",
                            propertyTable.CreateQuery());

                    }
               
               
                Debug.WriteLine("Panding Sync Data: {0}", client.SyncContext.PendingOperations);
                // "Property",
                //  propertyTable.CreateQuery()



            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    Debug.WriteLine(exc.PushResult.Errors);
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                      //  Update failed, reverting to server's copy.
                    await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else if (error.Status == HttpStatusCode.PreconditionFailed)
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                        Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                      
                    }
             
                }
            }
        }
#endif

    }




}
