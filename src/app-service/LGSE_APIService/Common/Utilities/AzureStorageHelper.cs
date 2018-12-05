using System;

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace LGSE_APIService.Common.Utilities
{
    public static class AzureStorageHelper
    {
        /// <summary>
        /// Getting the storage account name
        /// </summary>
        /// <returns></returns>
        private static string GetStorageAccountName()
        {
            return ConfigurationManager.AppSettings["AFStorageActName"].ToString();
        }
        /// <summary>
        /// Getting the storage account key
        /// </summary>
        /// <returns></returns>
        private static string GetStorageAccountKey()
        {
            return ConfigurationManager.AppSettings["AFStorageActKey"].ToString();
        }
        /// <summary>
        /// Getting the connection string 
        /// </summary>
        /// <returns></returns>
        private static CloudStorageAccount GetConnectionString()
        {
            try
            {
                string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", GetStorageAccountName(), GetStorageAccountKey());
                return CloudStorageAccount.Parse(connectionString);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Get the blob reference by connecting to the container
        /// </summary>
        /// <param name="containername"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static void  SendMessageToQueue(string inputMessage)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = GetConnectionString();
                var client = cloudStorageAccount.CreateCloudQueueClient();
                CloudQueue queue = client.GetQueueReference(ConfigurationManager.AppSettings["AFQueueName"]);
                CloudQueueMessage message = new CloudQueueMessage(inputMessage);
                queue.AddMessage(message);
            }
            catch (Exception e)
            {
               throw e;
            }
        }
    }
}