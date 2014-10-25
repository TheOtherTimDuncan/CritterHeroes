using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AR.RescueGroups.Mappings
{
    public interface IRescueGroupsMapping<T> where T : class
    {
        string ObjectType
        {
            get;
        }

        bool IsPrivate
        {
            get;
        }

        IEnumerable<T> ToModel(IEnumerable<JProperty> tokens);
    }
}
