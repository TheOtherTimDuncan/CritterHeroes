using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class CritterSearchResult
    {
        [JsonProperty(PropertyName = "animalID")]
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

        [JsonProperty(PropertyName = "animalLocationID")]
        public string LocationID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "locationName")]
        public string LocationName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalPictures")]
        public CritterPictureSource[] PictureSources
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalCourtesy")]
        public string Courtesy
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalDescription")]
        public string Description
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalGeneralAge")]
        public string GeneralAge
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalSpecialneeds")]
        public string SpecialNeeds
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalSpecialneedsDescription")]
        public string SpecialNeedsDescription
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalSpecialDiet")]
        public string SpecialDiet
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalReceivedDate")]
        public string ReceivedDate
        {
            get;
            set;
        }
    }
}
