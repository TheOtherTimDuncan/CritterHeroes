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
            : this(business)
        {
            this.GroupID = groupID;
            this.Group = null;
        }

        internal BusinessGroup(Business business, Group group)
            : this(business)
        {
            ThrowIf.Argument.IsNull(group, nameof(group));

            this.GroupID = group.ID;
            this.Group = group;
        }

        private BusinessGroup(Business business)
        {
            ThrowIf.Argument.IsNull(business, nameof(business));

            this.BusinessID = business.ID;
            this.Business = business;
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
