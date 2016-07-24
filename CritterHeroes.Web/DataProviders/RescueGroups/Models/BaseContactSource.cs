using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.JsonConverters;
using Newtonsoft.Json;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class BaseContactSource : BaseSource
    {
        [JsonProperty(PropertyName = "contactID")]
        public override int ID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactEmail")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string Email
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactAddress")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string Address
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactCity")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string City
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactState")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string State
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactPostalcode")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string PostalCode
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "contactPlus4")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string PostalPlus4
        {
            get;
            set;
        }

        [JsonIgnore]
        public string Zip
        {
            get
            {
                if (PostalCode.IsNullOrEmpty())
                {
                    return null;
                }
                return PostalCode + PostalPlus4;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    PostalCode = value?.Substring(0, 5);
                    if (value.Length > 5)
                    {
                        PostalPlus4 = value.Substring(5);
                    }
                }
                else
                {
                    PostalCode = null;
                    PostalPlus4 = null;
                }
            }
        }

        [JsonProperty(PropertyName = "contactPhoneWork")]
        [JsonConverter(typeof(PhoneConverter))]
        public string PhoneWork
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactPhoneWorkExt")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string PhoneWorkExtension
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactFax")]
        [JsonConverter(typeof(PhoneConverter))]
        public string PhoneFax
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contactGroups")]
        public string Groups
        {
            get;
            set;
        }

        [JsonIgnore]
        public IEnumerable<string> GroupNames
        {
            get
            {
                return Groups.NullSafeSplit(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
