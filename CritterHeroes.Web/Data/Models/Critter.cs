using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class Critter
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
    }
}
