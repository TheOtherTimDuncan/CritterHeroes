using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class CritterStatusSource
    {
        public CritterStatusSource(string id, string name, string description)
        {
            ThrowIf.Argument.IsNullOrEmpty(id, nameof(id));
            ThrowIf.Argument.IsNullOrEmpty(name, nameof(name));

            this.ID = id;
            this.Name = name;
            this.Description = description;
        }

        [JsonProperty(PropertyName = "statusID")]
        public string ID
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "statusName")]
        public string Name
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "statusDescription")]
        public string Description
        {
            get;
            private set;
        }
    }
}
