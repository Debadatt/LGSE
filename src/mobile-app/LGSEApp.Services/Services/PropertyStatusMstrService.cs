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
  public  class PropertyStatusMstrService: BaseService
    {
        static PropertyStatusMstrService defaultInstance = new PropertyStatusMstrService();

        private PropertyStatusMstrService()
        {
           
        }

        public static PropertyStatusMstrService DefaultManager
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
            get { return propertyStatusMstrTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<PropertyStatusMstr>; }
        }
        public async Task<ObservableCollection<PropertyStatusMstr>> GetPropertyStatusItemsAsync(bool syncItems = false)
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

                IEnumerable<PropertyStatusMstr> items = await propertyStatusMstrTable.Where(s => s.Deleted == false).OrderBy(s=> s.DisplayOrder)
                     .ToEnumerableAsync();
                //  .Where(todoItem => todoItem.Status == 0)
                // .Where(todoItem => !todoItem.)
                return new ObservableCollection<PropertyStatusMstr>(items);
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

        public async Task SaveTaskAsync(PropertyStatusMstr item)
        {
            try
            {
                if (item.Id == null)
                {
                    await propertyStatusMstrTable.InsertAsync(item);
                }
                else
                {
                    await propertyStatusMstrTable.UpdateAsync(item);
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
                //PurgesAndClearsAllKeys()
              //  var propertycount = await propertyStatusMstrTable.ToEnumerableAsync();
            //    Debug.Write(propertycount.Count());
            //    await propertyStatusMstrTable.PullAsync(null, null);
                await propertyStatusMstrTable.PurgeAsync(null, null, true, CancellationToken.None);
             //    propertycount = await propertyStatusMstrTable.ToEnumerableAsync();
             //   Debug.Write(propertycount.Count());
            }
            catch(Exception ex)
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

                //Pull Data on Azure Database
                //   await this.client.SyncContext.PushAsync();
                //    var propertycount = await propertyStatusMstrTable.ToEnumerableAsync();
                //    Debug.Write(propertycount.Count());
              
                    await propertyStatusMstrTable.PullAsync(
                       //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                       //Use a different query name for each unique query in your program
                       null,
                        null);
              


               //  propertycount = await propertyStatusMstrTable.ToEnumerableAsync();
            //    Debug.Write(propertycount.Count());
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
