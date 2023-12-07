using System;

namespace Capstone.Utils
{
    public class SqlUtil
    {
        public static string NullableString(object dbValue)
        {
            if (dbValue is DBNull)
            {
                return null;
            }
            else
            {
                return Convert.ToString(dbValue);
            }
        }
    }
}
