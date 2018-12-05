using BingMapsSDSToolkit.GeocodeDataflowAPI;
using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using Microsoft.Extensions.Logging;

namespace LGSE_FunctionsHost.Utilities
{
    public class DBUtility
    {
        public void UpdateSuccessLatLong(List<GeocodeEntity> geoCodeEntities,string incId)
        {
            // update in database all the succeed entities.
            List<string> dbOperations = new List<string>();
            foreach (var itemGeoCode in geoCodeEntities)
            {
                var lat= itemGeoCode.GeocodeResponse[0].GeocodePoint[0].Latitude;
                var lang = itemGeoCode.GeocodeResponse[0].GeocodePoint[0].Longitude;
                dbOperations.Add("update Properties set Latitude = '"+ lat + "', Longitude='"+lang+ "' where IncidentId = '"+incId+"'"+ " and MPRN='"+itemGeoCode.Id+"'");
            }
            using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("MS_TableConnectionString", EnvironmentVariableTarget.Process)))
            {
                 
                connection.Open();
               // SqlTransaction transaction = connection.BeginTransaction();
                foreach (string commandString in dbOperations)
                {
                    SqlCommand cmd = new SqlCommand(commandString, connection);
                    cmd.ExecuteNonQuery();
                }
                //transaction.Commit();
            }
            LGSEBingMapFunction.logger.LogInformation("Lat and Lang updated to database successfully.");
        }
    }
}
