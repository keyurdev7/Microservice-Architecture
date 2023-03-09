using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Application.Behaviours
{
    public class Common
    {
        #region Methods

        #region Get null value if pass paramter is "0" or "null"
        // Returns null if pass paramter is "0" or "null"
        public static long? GetNullLong(long? i)
        {
            if (i == null || i == 0)
            {
                return null;
            }
            else
            {
                return i;
            }
        }
        #endregion

        #region Get null value if pass paramter is "0" or "null"
        // Returns null if pass paramter is "0" or "null"
        public static int? GetNullInt(int? i)
        {
            if (i == null || i == 0)
            {
                return null;
            }
            else
            {
                return i;
            }
        }
        #endregion

        //Returns null if if pass paramter is "" or "null"
        public static string GetNullString(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return null;
            }
            else
            {
                return param;
            }
        }
        #endregion
    }
}
