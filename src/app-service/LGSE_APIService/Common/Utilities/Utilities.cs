
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGSE_APIService.Utilities
{
    /// <summary>
    /// Holds the generic utility methods required
    /// </summary>
    public class Utilities
    {  
        /// <summary>
        /// Checks 2 byte arrays bit by bit
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool slowEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }

     
    }
}
