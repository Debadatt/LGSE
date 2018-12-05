using LGSE_APIService.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LGSE_APIService.Common.Utilities
{
    public static class CSVHelper
    {
        /// <summary>
        /// writes properties 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        public static void AppendValue(StringBuilder sb, string value, bool first, bool last)
        {
            if (value == null)
                value = string.Empty;
            if (!value.Contains('\"'))
            {
                if (!first)
                    sb.Append(',');

                sb.Append(value);

                if (last)
                    sb.AppendLine();
            }
            else
            {
                if (!first)
                    sb.Append(",\"");
                else
                    sb.Append('\"');

                sb.Append(value.Replace("\"", "\"\""));

                if (last)
                    sb.AppendLine("\"");
                else
                    sb.Append('\"');
            }
        }
        
    }
}