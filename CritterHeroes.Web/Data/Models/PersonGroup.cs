using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class PersonGroup
    {
        protected PersonGroup()
        {
        }

        internal PersonGroup(Person person, int groupID)
        {
            ThrowIf.Argument.IsNull(person, nameof(person));

            this.PersonID = person.ID;
            this.Person = person;

            this.GroupID = groupID;
            this.Group = null;
        }

        internal PersonGroup(Group group, int personID)
        {
            ThrowIf.Argument.IsNull(group, nameof(group));

            this.PersonID = personID;
            this.Person = null;

            this.GroupID = group.ID;
            this.Group = group;
        }

        public int PersonID
        {
            get;
            private set;
        }

        public virtual Person Person
        {
            get;
            private set;
        }

        public int GroupID
        {
            get;
            private set;
        }

        public virtual Group Group
        {
            get;
            private set;
        }
    }
}
