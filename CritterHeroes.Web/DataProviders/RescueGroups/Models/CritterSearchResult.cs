using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class CritterSearchResult
    {
        [JsonProperty(PropertyName ="animalID")]
        public int ID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalName")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalStatusID")]
        public string StatusID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalStatus")]
        public string Status
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalSpecies")]
        public string Species
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalPrimaryBreedID")]
        public string PrimaryBreedID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalPrimaryBreed")]
        public string PrimaryBreed
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalSex")]
        public string Sex
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalRescueID")]
        public string RescueID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalUpdatedDate")]
        public string LastUpdated
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalCreatedDate")]
        public string Created
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalFosterID")]
        public string FosterContactID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "fosterFirstname")]
        public string FosterFirstName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "fosterLastname")]
        public string FosterLastName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "fosterEmail")]
        public string FosterEmail
        {
            get;
            set;
        }
    }
}
