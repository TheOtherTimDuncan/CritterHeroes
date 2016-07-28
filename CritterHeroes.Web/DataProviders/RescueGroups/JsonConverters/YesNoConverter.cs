using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.JsonConverters
{
    public class YesNoConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            string yesno = reader.Value.ToString();

            if (yesno.IsNullOrEmpty())
            {
                return null;
            }

            return yesno.SafeEquals("yes");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((bool)value ? "Yes" : "No");
        }
    }
}
