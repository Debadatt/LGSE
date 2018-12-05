using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.Common.Utilities
{
    /// <summary>
    /// LGSE logger class.
    ///
    /// </summary>
    public static class LGSELogger
    {
        // Make sure diagnostics logs are enabled in azure  in the path Home
        // for example for the dev instance Resource groups-> LGSE-DEV -> LGSE-API-DEV - Diagnostics logs.
        // if Application Logging(blob) enabled then you can find the logs in the storage account associated.
        public static void Information(string message)
        {
            System.Diagnostics.Trace.TraceInformation(message);
        }
        public static void Information(string format, params object[] args)
        {
            System.Diagnostics.Trace.TraceInformation(format, args);
        }
        public static void Error(Exception ex)
        {
            //System.Diagnostics.Trace.TraceError("LGSE API Error" + ex.Message +"\n"+ex.StackTrace.ToString()+"\n"+ex.InnerException.Message);
            //System.Diagnostics.Trace.TraceError("LGSE Full Error:{0} \n LGSE error Message:{1}", ex,ex.Message) ;
            System.Diagnostics.Trace.TraceError("LGSE API Error:{0}", ex);
        }
    }
}