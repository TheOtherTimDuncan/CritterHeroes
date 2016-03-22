using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.JsonConverters;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class PersonSource : BaseContactSource
    {
        [JsonProperty(PropertyName = "contactFirstName")]
        public string FirstName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactLastName")]
        public string LastName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactPhoneHome")]
        [JsonConverter(typeof(PhoneConverter))]
        public string PhoneHome
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactPhoneCell")]
        [JsonConverter(typeof(PhoneConverter))]
        public string PhoneCell
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactActive")]
        [JsonConverter(typeof(YesNoConverter))]
        public bool IsActive
        {
            get;
            set;
        }
    }
}
