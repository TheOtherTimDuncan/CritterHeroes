using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class BusinessGroup
    {
        protected BusinessGroup()
        {
        }

        internal BusinessGroup(Business business, int groupID)
        {
            ThrowIf.Argument.IsNull(business, nameof(business));

            this.BusinessID = business.ID;
            this.Business = business;

            this.GroupID = groupID;
            this.Group = null;
        }

        internal BusinessGroup(Group group, int businessID)
        {
            ThrowIf.Argument.IsNull(group, nameof(group));

            this.BusinessID = businessID;
            this.Business = null;

            this.GroupID = group.ID;
            this.Group = group;
        }

        public int BusinessID
        {
            get;
            private set;
        }

        public virtual Business Business
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
