﻿using System;
using System.Collections.Generic;
using CritterHeroes.Web.DataProviders.RescueGroups.JsonConverters;
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
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalStatusID")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string StatusID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalStatus")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string Status
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalSpecies")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
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
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string RescueID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalUpdatedDate")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? LastUpdated
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalCreatedDate")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Created
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
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string FosterFirstName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "fosterLastname")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string FosterLastName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "fosterEmail")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
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
        [JsonConverter(typeof(EmptyToNullStringConverter))]
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
        [JsonConverter(typeof(YesNoConverter))]
        public bool? IsCourtesy
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalDescription")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string Description
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalGeneralAge")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string GeneralAge
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalSpecialneeds")]
        [JsonConverter(typeof(YesNoConverter))]
        public bool? HasSpecialNeeds
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalSpecialneedsDescription")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string SpecialNeedsDescription
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalSpecialDiet")]
        [JsonConverter(typeof(YesNoConverter))]
        public bool? HasSpecialDiet
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalReceivedDate")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ReceivedDate
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalBirthdate")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? BirthDate
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalBirthdateExact")]
        [JsonConverter(typeof(YesNoConverter))]
        public bool? IsBirthDateExact
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalColorID")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string ColorID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalColor")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string Color
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalKillDate")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? EuthanasiaDate
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "animalKillReason")]
        [JsonConverter(typeof(EmptyToNullStringConverter))]
        public string EuthanasiaReason
        {
            get;
            set;
        }
    }
}
