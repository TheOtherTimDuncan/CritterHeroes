using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class Group
    {
        protected Group()
        {
        }

        public Group(string name)
        {
            ThrowIf.Argument.IsNullOrEmpty(name, nameof(name));

            this.Name = name;

            Persons = new List<PersonGroup>();
        }

        public int ID
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }

        public virtual ICollection<PersonGroup> Persons
        {
            get;
            private set;
        }

        public void AddPerson(int personID)
        {
            PersonGroup personGroup = new PersonGroup(this, personID);
            Persons.Add(personGroup);
        }
    }
}
