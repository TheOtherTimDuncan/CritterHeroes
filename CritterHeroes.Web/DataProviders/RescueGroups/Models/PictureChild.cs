using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class PictureChild
    {
        [JsonProperty(PropertyName = "fileSize")]
        public long FileSize
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "resolutionX")]
        public int Width
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "resolutionY")]
        public int Height
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "url")]
        public string Url
        {
            get;
            set;
        }
    }
}
