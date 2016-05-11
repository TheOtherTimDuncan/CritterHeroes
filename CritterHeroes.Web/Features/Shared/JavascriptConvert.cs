using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TOTD.Utility.EnumerableHelpers;

namespace CritterHeroes.Web.Features.Shared
{
    public static class JavascriptConvert
    {
        public static IHtmlString SerializeObject<T>(IEnumerable<T> value)
        {
            if (value.IsNullOrEmpty())
            {
                return new HtmlString("[]");
            }

            return GetSerializedObject(value);
        }

        public static IHtmlString SerializeObject(object value)
        {
            return GetSerializedObject(value);
        }

        private static IHtmlString GetSerializedObject(object value)
        {
            using (StringWriter stringWriter = new StringWriter())
            using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter))
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    // Let's use camelCasing as is common practice in JavaScript
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                jsonWriter.Formatting = Formatting.Indented;

                // Let's play it safe and make sure the object properties have quotes
                jsonWriter.QuoteName = true;
                serializer.Serialize(jsonWriter, value);

                string json = stringWriter.ToString();

                return new HtmlString(json);
            }
        }
    }
}
