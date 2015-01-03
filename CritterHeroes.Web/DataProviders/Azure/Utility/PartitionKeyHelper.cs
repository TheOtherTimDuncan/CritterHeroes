using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterHeroes.Web.DataProviders.Azure.Utility
{
    public static class PartitionKeyHelper
    {
        public static string GetLoggingKey()
        {
            DateTime now = DateTime.UtcNow;
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0, DateTimeKind.Utc).Ticks.ToString("d19");
        }

        public static string GetLoggingKeyForDate(DateTime logDateUtc)
        {
            return new DateTime(logDateUtc.Year, logDateUtc.Month, logDateUtc.Day, logDateUtc.Hour, logDateUtc.Minute, 0, DateTimeKind.Utc).Ticks.ToString("d19");
        }
    }
}
