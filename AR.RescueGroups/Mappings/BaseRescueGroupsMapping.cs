using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AR.RescueGroups.Mappings
{
    public abstract class BaseRescueGroupsMapping<T> : IRescueGroupsMapping<T> where T: class
    {
        public abstract string ObjectType
        {
            get;
        }

        public virtual bool IsPrivate
        {
            get
            {
                return false;
            }
        }

        public abstract IEnumerable<T> ToModel(IEnumerable<JProperty> tokens);
    }
}
