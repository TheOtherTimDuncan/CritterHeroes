using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class Critter : IPreserveHistory
    {
        protected Critter()
        {
        }

        public Critter(string name, CritterStatus status, Breed breed, Guid organizationID, int rescueGroupsID)
            : this(name, status, breed, organizationID)
        {
            this.RescueGroupsID = rescueGroupsID;
        }

        public Critter(string name, CritterStatus status, Breed breed, Guid organizationID)
        {
            ThrowIf.Argument.IsNullOrEmpty(name, nameof(name));

            ChangeStatus(status);
            ChangeBreed(breed);

            this.Name = name;

            this.OrganizationID = organizationID;

            this.WhenCreated = DateTimeOffset.UtcNow;
            this.WhenUpdated = this.WhenCreated;

            this.Pictures = new List<CritterPicture>();
        }

        public int ID
        {
            get;
            private set;
        }

        public int? RescueGroupsID
        {
            get;
            private set;
        }

        public Guid OrganizationID
        {
            get;
            private set;
        }

        public virtual Organization Organization
        {
            get;
            private set;
        }

        public int StatusID
        {
            get;
            private set;
        }

        public virtual CritterStatus Status
        {
            get;
            private set;
        }

        public int? LocationID
        {
            get;
            private set;
        }

        public virtual Location Location
        {
            get;
            private set;
        }

        public DateTimeOffset? RescueGroupsLastUpdated
        {
            get;
            set;
        }

        public DateTimeOffset? RescueGroupsCreated
        {
            get;
            set;
        }

        public DateTimeOffset WhenCreated
        {
            get;
            set;
        }

        public DateTimeOffset WhenUpdated
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int BreedID
        {
            get;
            private set;
        }

        public virtual Breed Breed
        {
            get;
            private set;
        }

        public string Sex
        {
            get;
            set;
        }

        public string RescueID
        {
            get;
            set;
        }

        public DateTimeOffset? ReceivedDate
        {
            get;
            set;
        }

        public bool IsCourtesy
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string GeneralAge
        {
            get;
            set;
        }

        public bool HasSpecialNeeds
        {
            get;
            set;
        }

        public string SpecialNeedsDescription
        {
            get;
            set;
        }

        public bool HasSpecialDiet
        {
            get;
            set;
        }

        public int? FosterID
        {
            get;
            private set;
        }

        public virtual Person Foster
        {
            get;
            private set;
        }

        public DateTime? BirthDate
        {
            get;
            set;
        }

        public bool? IsBirthDateExact
        {
            get;
            set;
        }

        public DateTime? EuthanasiaDate
        {
            get;
            set;
        }

        public string EuthanasiaReason
        {
            get;
            set;
        }

        public int? ColorID
        {
            get;
            private set;
        }

        public virtual CritterColor Color
        {
            get;
            private set;
        }

        public bool? IsMicrochipped
        {
            get;
            set;
        }

        public bool? IsOkWithDogs
        {
            get;
            set;
        }

        public bool? IsOkWithKids
        {
            get;
            set;
        }

        public bool? IsOkWithCats
        {
            get;
            set;
        }

        public bool? OlderKidsOnly
        {
            get;
            set;
        }

        public string Notes
        {
            get;
            set;
        }

        public virtual ICollection<CritterPicture> Pictures
        {
            get;
            private set;
        }

        object IPreserveHistory.ID
        {
            get
            {
                return this.ID;
            }
        }

        public void ChangeBreed(int breedID)
        {
            this.BreedID = breedID;
            this.Breed = null;
        }

        public void ChangeBreed(Breed breed)
        {
            ThrowIf.Argument.IsNull(breed, nameof(breed));
            this.BreedID = breed.ID;
            this.Breed = breed;
        }

        public void ChangeStatus(int statusID)
        {
            this.StatusID = statusID;
            this.Status = null;
        }

        public void ChangeStatus(CritterStatus status)
        {
            ThrowIf.Argument.IsNull(status, nameof(status));
            this.StatusID = status.ID;
            this.Status = status;
        }

        public void ChangeFoster(Person person)
        {
            ThrowIf.Argument.IsNull(person, nameof(person));
            this.FosterID = person.ID;
            this.Foster = person;
        }

        public void RemoveFoster()
        {
            this.Foster = null;
            this.FosterID = null;
        }

        public CritterPicture AddPicture(Picture picture)
        {
            CritterPicture critterPicture = new CritterPicture(this, picture);
            Pictures.Add(critterPicture);
            return critterPicture;
        }

        public void ChangeLocation(int locationID)
        {
            this.Location = null;
            this.LocationID = locationID;
        }

        public void ChangeLocation(Location location)
        {
            ThrowIf.Argument.IsNull(location, nameof(location));

            this.Location = location;
            this.LocationID = location.ID;
        }

        public void RemoveLocation()
        {
            this.Location = null;
            this.LocationID = null;
        }

        public void ChangeColor(int colorID)
        {
            this.ColorID = colorID;
            this.Color = null;
        }

        public void ChangeColor(CritterColor color)
        {
            ThrowIf.Argument.IsNull(color, nameof(color));

            this.ColorID = color.ID;
            this.Color = color;
        }

        public void RemoveColor()
        {
            ColorID = null;
            Color = null;
        }
    }
}
