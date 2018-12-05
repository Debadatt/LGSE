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
#endif


namespace LGSEApp.Services.Services
{
  public partial class PropertyUserStatusService: BaseService
    {
        static PropertyUserStatusService defaultInstance = new PropertyUserStatusService();

        private PropertyUserStatusService()
        {
            //            this.client = new MobileServiceClient(Constants.ApplicationURL, new MyHandler());
            //            // this.client = new MobileServiceClient(Constants.ApplicationURL);

//#if OFFLINE_SYNC_ENABLED
//            var store = new MobileServiceSQLiteStore(offlineDbPath);
//            store.DefineTable<PropertyUserStatus>();

//            //Initializes the SyncContext using the default IMobileServiceSyncHandler.aaaaaa
//            client.SyncContext.InitializeAsync(store);

//            propertyUserStatusTable = client.GetSyncTable<PropertyUserStatus>();
//#else
//            this.propertyUserStatusTable = client.GetTable<PropertyUserStatus>();
          
//#endif
        }

        public static PropertyUserStatusService DefaultManager
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
            get { return propertyUserStatusTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<PropertyUserStatus>; }
        }
        //bool isPrority, int isProgress,
        public async Task<ObservableCollection<PropertyUserStatus>> GetPropertyUserStatusItemsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED

                if (syncItems)
                {
                    try
                    {
                        await this.SyncAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }

#endif

                IEnumerable<PropertyUserStatus> items = await propertyUserStatusTable
                  
                     //   .Where(todoItem => todoItem.PriorityCustomer == isPrority)
                     .ToEnumerableAsync();
                //  .Where(todoItem => todoItem.Status == 0)
                // .Where(todoItem => !todoItem.)
                return new ObservableCollection<PropertyUserStatus>(items);
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

        public async Task SaveTaskAsync(PropertyUserStatus item)
        {
            try
            {
                Debug.WriteLine("Panding Sync Data: {0}", client.SyncContext.PendingOperations);
                var data = await propertyUserStatusTable.OrderByDescending(i => i.StatusChangedOn).ToListAsync();
                Debug.WriteLine(data.Count());
                if (item.Id == null)
                {
                    await propertyUserStatusTable.InsertAsync(item);
                }
                else
                {
                    await propertyUserStatusTable.UpdateAsync(item);
                }
                Debug.WriteLine("Panding Sync Data: {0}", client.SyncContext.PendingOperations);
              //  await SyncAsync();
                 data = await propertyUserStatusTable.OrderByDescending(i=>i.StatusChangedOn).ToListAsync();
                Debug.WriteLine(data.Count());
                Debug.WriteLine("Panding Sync Data: {0}", client.SyncContext.PendingOperations);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public static int PandingDataCount()
        {
            Debug.WriteLine(client.SyncContext.PendingOperations);
            return Convert.ToInt32(client.SyncContext.PendingOperations);
        
        }
        public async Task DeleteTaskAsync()
        {
            try
            {
                await propertyUserStatusTable.PurgeAsync(null, null, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                //Debug.WriteLine("Panding Sync Data: {0}", client.SyncContext.PendingOperations);
                await client.SyncContext.PushAsync();
                //Debug.WriteLine("Panding Sync Data: {0}", client.SyncContext.PendingOperations);

                //await propertyUserStatusTable.PullAsync(
                //    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //    //Use a different query name for each unique query in your program
                //    null,
                //    null);



            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            //Simple error/ conflict handling.A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else 
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
