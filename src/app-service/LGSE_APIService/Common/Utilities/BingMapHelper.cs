using BingMapsRESTToolkit;
using LGSE_APIService.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LGSE_APIService.Common.Utilities
{
    public static class BingMapHelper
    {
        public static async Task<string> GetLatLang(string query)
        {
            try
            {
                string reusltLatLang = string.Empty;
                //Create a request.
                var request = new GeocodeRequest()
                {
                    Culture = "en-GB",
                    UserIp = "127.0.0.1",
                    //Query = "COLIEMORE HOUSE, COLI, RENNEY ROAD, DOWN THOMAS, PLYMOUTH, PL9 0BQ",
                    Query = query,
                    //Address = new SimpleAddress() {

                    //},
                    IncludeIso2 = true,
                    IncludeNeighborhood = true,
                    MaxResults = 1,
                    BingMapsKey = "At7iPrYTWNA8YXAb8tUnI1oJ1OTzbmSnQ23xqdw0HqyOrM_m5WjaPI8u_AI0uPk8" //"Apiu5N8o7_72FUmE33bvupI31kyg2Wz7sDIQviRwPttMKfu7Pmmhf219uIso-1ra"
                };

                //Process the request by using the ServiceManager.
                var response = await ServiceManager.GetResponseAsync(request);

                if (response != null &&
                    response.ResourceSets != null &&
                    response.ResourceSets.Length > 0 &&
                    response.ResourceSets[0].Resources != null &&
                    response.ResourceSets[0].Resources.Length > 0)
                {
                    var result = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Location;
                    string lat = result.Point.Coordinates[0].ToString();
                    string lang = result.Point.Coordinates[1].ToString();
                    reusltLatLang = lat + "|" + lang;
                    //Do something with the result.
                    return reusltLatLang;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
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
