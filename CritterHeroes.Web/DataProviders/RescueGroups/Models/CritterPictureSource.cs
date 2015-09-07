using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class CritterPictureSource
    {
        [JsonProperty(PropertyName = "mediaID")]
        public string ID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "mediaOrder")]
        public int? DisplayOrder
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "lastUpdated")]
        public string LastUpdated
        {
            get;
            set;
        }

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

        [JsonProperty(PropertyName = "fileNameFullsize")]
        public string Filename
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "urlSecureFullsize")]
        public string Url
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "large")]
        public PictureChild LargePicture
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "small")]
        public PictureChild SmallPicture
        {
            get;
            set;
        }
    }
}
