using System;
using System.Collections.Generic;
using BingMapsSDSToolkit.GeocodeDataflowAPI;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LGSE_FunctionsHost
{
    public static class LGSEBingMapFunction
    {
        public static ILogger logger { get; set; }
        [FunctionName("Function1")]
        public static void Run([QueueTrigger("lgsequeuedev", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            try
            {
                logger = log;
                logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
                BingMapUtility bingMapUtility = new BingMapUtility();
                IncidentRequest incReq = JsonConvert.DeserializeObject<IncidentRequest>(myQueueItem);
                var entities = bingMapUtility.MPRNStoEntitiesConverter(incReq);
                bingMapUtility.GeoCode(entities,incReq.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
            }
        }
    }
}
