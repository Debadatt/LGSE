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
    public class PropertySubStatusMstrService : BaseService
    {
        static PropertySubStatusMstrService defaultInstance = new PropertySubStatusMstrService();

        private PropertySubStatusMstrService()
        {
//            this.client = new MobileServiceClient(Constants.ApplicationURL, new MyHandler());
//            // this.client = new MobileServiceClient(Constants.ApplicationURL);

//#if OFFLINE_SYNC_ENABLED
//            var store = new MobileServiceSQLiteStore(offlineDbPath);
//            store.DefineTable<PropertySubStatusMstr>();

//            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
//            this.client.SyncContext.InitializeAsync(store);

//            this.propertySubStatusMstrTable = client.GetSyncTable<PropertySubStatusMstr>();
//#else
//            this.propertySubStatusMstrTable = client.GetTable<PropertySubStatusMstr>();
            
          
//#endif
        }

        public static PropertySubStatusMstrService DefaultManager
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
            get { return propertySubStatusMstrTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<PropertySubStatusMstr>; }
        }
        public async Task<ObservableCollection<PropertySubStatusMstr>> GetPropertySubStatusItemsAsync(string id, bool syncItems = false)
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

                IEnumerable<PropertySubStatusMstr> items = await propertySubStatusMstrTable
                    .Where(s => s.PropertyStatusMstrsId == id && s.Deleted==false)
                    .OrderBy(s => s.DisplayOrder)
                     .ToEnumerableAsync();
                //  .Where(todoItem => todoItem.Status == 0)
                // .Where(todoItem => !todoItem.)
                return new ObservableCollection<PropertySubStatusMstr>(items);
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

        public async Task SaveTaskAsync(PropertySubStatusMstr item)
        {
            try
            {
                if (item.Id == null)
                {
                    await propertySubStatusMstrTable.InsertAsync(item);
                }
                else
                {
                    await propertySubStatusMstrTable.UpdateAsync(item);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
        public async Task DeleteTaskAsync()
        {
            try
            {
                await propertySubStatusMstrTable.PurgeAsync(null, null, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync(bool IsSync = false)
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                //Pull Data on Azure Database
                //  await this.client.SyncContext.PushAsync();
            
                    await propertySubStatusMstrTable.PullAsync(
                       //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                       //Use a different query name for each unique query in your program
                       null, null);
               
                //"PropertySubStatusMstr",
                //    propertySubStatusMstrTable.CreateQuery()



            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
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
                        //Update failed, reverting to server's copy.
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
