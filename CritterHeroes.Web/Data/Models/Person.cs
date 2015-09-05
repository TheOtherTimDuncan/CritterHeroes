using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Data.Models
{
    public class Person
    {
        public Person()
        {
            Groups = new List<PersonGroup>();
            PhoneNumbers = new List<PersonPhone>();
            IsActive = true;
        }

        public int ID
        {
            get;
            private set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string Zip
        {
            get;
            set;
        }

        public string RescueGroupsID
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public virtual ICollection<PersonGroup> Groups
        {
            get;
            private set;
        }

        public virtual ICollection<PersonPhone> PhoneNumbers
        {
            get;
            private set;
        }

        public void AddGroup(int groupID)
        {
            PersonGroup personGroup = new PersonGroup(this, groupID);
            Groups.Add(personGroup);
        }

        public PersonPhone AddPhoneNumber(string phoneNumber, string phoneExtension, int phoneTypeID)
        {
            PersonPhone personPhone = new PersonPhone(this, phoneTypeID, phoneNumber, phoneExtension);
            PhoneNumbers.Add(personPhone);
            return personPhone;
        }

        public PersonPhone AddPhoneNumber(string phoneNumber, string phoneExtension, PhoneType phoneType)
        {
            PersonPhone personPhone = new PersonPhone(this, phoneType, phoneNumber, phoneExtension);
            PhoneNumbers.Add(personPhone);
            return personPhone;
        }
    }
}
