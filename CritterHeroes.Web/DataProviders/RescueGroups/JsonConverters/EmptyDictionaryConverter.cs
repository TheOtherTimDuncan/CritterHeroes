using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.RescueGroups.JsonConverters
{
    public class EmptyDictionaryConverter : JsonConverter
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
            return objectType == typeof(Dictionary<,>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.StartArray)
            {
                reader.Skip();
                return null;
            }

            Type[] valueTypes = objectType.GetGenericArguments();
            Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(valueTypes);
            object dictionary = Activator.CreateInstance(dictionaryType);
            serializer.Populate(reader, dictionary);

            return dictionary;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
