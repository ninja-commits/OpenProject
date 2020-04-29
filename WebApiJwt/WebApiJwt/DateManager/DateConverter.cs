using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiJwt.DateManager
{
    public class DateConverter
    {
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

    }
}