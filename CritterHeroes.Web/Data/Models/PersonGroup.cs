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
            : this(person)
        {
            this.GroupID = groupID;
            this.Group = null;
        }

        internal PersonGroup(Person person, Group group)
            : this(person)
        {
            ThrowIf.Argument.IsNull(group, nameof(group));

            this.GroupID = group.ID;
            this.Group = group;
        }

        private PersonGroup(Person person)
        {
            ThrowIf.Argument.IsNull(person, nameof(person));

            this.PersonID = person.ID;
            this.Person = person;
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
