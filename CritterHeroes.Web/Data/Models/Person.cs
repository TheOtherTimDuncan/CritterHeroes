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

        public virtual ICollection<PersonGroup> Groups
        {
            get;
            private set;
        }

        public void AddGroup(int groupID)
        {
            PersonGroup personGroup = new PersonGroup(this, groupID);
            Groups.Add(personGroup);
        }
    }
}
