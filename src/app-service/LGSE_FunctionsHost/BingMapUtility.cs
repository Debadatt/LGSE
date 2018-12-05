using BingMapsSDSToolkit.GeocodeDataflowAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using LGSE_FunctionsHost.Utilities;

namespace LGSE_FunctionsHost
{
    public class BingMapUtility
    {
        public void GeoCode(List<GeocodeEntity> reqEntities,string incId)
        {
            //This sample shows how to perform a back geocode request from a console app.
            // var bingMapsKey = "Apiu5N8o7_72FUmE33bvupI31kyg2Wz7sDIQviRwPttMKfu7Pmmhf219uIso-1ra";
            var bingMapsKey=Environment.GetEnvironmentVariable("BingMapKey", EnvironmentVariableTarget.Process);
            var geocodeFeed = new GeocodeFeed()
            {
                Entities = reqEntities
            };
            LGSEBingMapFunction.logger.LogInformation(string.Format("Creating batch geocode job consisting of {0} entities.\n", geocodeFeed.Entities.Count));
            var geocodeManager = new BatchGeocodeManager();
            var r = geocodeManager.Geocode(geocodeFeed, bingMapsKey).GetAwaiter().GetResult();

            if (!string.IsNullOrEmpty(r.Error))
            {
                LGSEBingMapFunction.logger.LogInformation("Error: " + r.Error);
            }
            else
            {
                LGSEBingMapFunction.logger.LogInformation("Batch geocode job complete:\n");
                DBUtility dBUtility = new DBUtility();
                if (r.Succeeded != null && r.Succeeded.Entities != null)
                {
                    LGSEBingMapFunction.logger.LogInformation(string.Format("Succeeded: {0}\n", r.Succeeded.Entities.Count));
                    LGSEBingMapFunction.logger.LogInformation("Query\tLatitude\tLongitude\n----------------------------------");
                    foreach (var e in r.Succeeded.Entities)
                    {
                        LGSEBingMapFunction.logger.LogInformation(string.Format("{0}\t{1}\t{2}", e.GeocodeResponse[0].Name, e.GeocodeResponse[0].GeocodePoint[0].Latitude, e.GeocodeResponse[0].GeocodePoint[0].Longitude));
                    }
                    dBUtility.UpdateSuccessLatLong(r.Succeeded.Entities, incId);
                }

                if (r.Failed != null && r.Failed.Entities != null)
                {
                    LGSEBingMapFunction.logger.LogInformation(string.Format("Failed: {0}\n", r.Failed.Entities.Count));

                    LGSEBingMapFunction.logger.LogInformation("Query\n-----------");
                    foreach (var e in r.Failed.Entities)
                    {
                        LGSEBingMapFunction.logger.LogInformation(e.GeocodeRequest.Query);
                    }
                }
            }

        }

        public List<GeocodeEntity> MPRNStoEntitiesConverter(IncidentRequest incReq)
        {
            List<GeocodeEntity> resGeoCodesEntList = new List<GeocodeEntity>();
            if (incReq.MPRNs != null)
            {
                foreach (var item in incReq.MPRNs)
                {
                    var query = GetQueryforMPRN(item);
                    resGeoCodesEntList.Add(new GeocodeEntity(query) {Id=item.MPRN });
                }
            }
            return resGeoCodesEntList;
        }

        public static string GetQueryforMPRN(PropertyRequest request)
        {
            string result = string.Empty;
            List<string> addList = new List<string>();
            if (!string.IsNullOrEmpty(request.BuildingName))
            {
                addList.Add(request.BuildingName);
            }
            if (!string.IsNullOrEmpty(request.SubBuildingName))
            {
                addList.Add(request.SubBuildingName);
            }
            if (!string.IsNullOrEmpty(request.MCBuildingName))
            {
                addList.Add(request.MCBuildingName);
            }
            if (!string.IsNullOrEmpty(request.MCSubBuildingName))
            {
                addList.Add(request.MCSubBuildingName);
            }
            if (!string.IsNullOrEmpty(request.BuildingNumber))
            {
                addList.Add(request.BuildingNumber);
            }
            if (!string.IsNullOrEmpty(request.PrincipalStreet))
            {
                addList.Add(request.PrincipalStreet);
            }
            if (!string.IsNullOrEmpty(request.DependentStreet))
            {
                addList.Add(request.DependentStreet);
            }
            if (!string.IsNullOrEmpty(request.DependentLocality))
            {
                addList.Add(request.DependentLocality);
            }
            if (!string.IsNullOrEmpty(request.LocalityName))
            {
                addList.Add(request.LocalityName);
            }
            if (!string.IsNullOrEmpty(request.PostTown))
            {
                addList.Add(request.PostTown);
            }
            if (!string.IsNullOrEmpty(request.Postcode))
            {
                addList.Add(request.Postcode);
            }
            result = string.Join(", ", addList);
            return result;
        }
    }
}
