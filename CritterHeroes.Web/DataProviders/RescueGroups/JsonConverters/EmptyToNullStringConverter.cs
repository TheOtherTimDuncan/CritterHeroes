using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.RescueGroups.JsonConverters
{
    public class EmptyToNullStringConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get
            {
                return false;
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

            if (String.Empty.Equals(reader.Value))
            {
                return null;
            }

            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
