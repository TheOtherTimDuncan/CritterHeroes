using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace CH.Azure.Utility
{
    public static class DynamicTableEntityExtensions
    {
        /// <summary>
        /// Returns the string value of the specified entity property or null if the property doesn't exist
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SafeGetEntityPropertyStringValue(this DynamicTableEntity entity, string key)
        {
            EntityProperty entityProperty;
            if (entity.Properties.TryGetValue(key, out entityProperty))
            {
                return entityProperty.StringValue;
            }
            return null;
        }

        /// <summary>
        /// Returns the DateTime value of the specified entity property or null if the property doesn't exist
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DateTime? SafeGetEntityPropertyDateTimeValue(this DynamicTableEntity entity, string key)
        {
            EntityProperty entityProperty;
            if (entity.Properties.TryGetValue(key, out entityProperty))
            {
                return entityProperty.DateTime;
            }
            return null;
        }
    }
}
