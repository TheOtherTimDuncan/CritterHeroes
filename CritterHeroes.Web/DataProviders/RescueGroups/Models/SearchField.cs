using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.EnumerableHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class SearchField
    {
        public SearchField(string name, params string[] supportingFields)
        {
            this.Name = name;
            this.IsSelected = true;
            this.SupportingFields = supportingFields;
        }

        public string Name
        {
            get;
        }

        public IEnumerable<string> SupportingFields
        {
            get;
        }

        public bool IsSelected
        {
            get;
            set;
        }

        public IEnumerable<string> FieldNames
        {
            get
            {
                yield return Name;

                if (!SupportingFields.IsNullOrEmpty())
                {
                    foreach (string name in SupportingFields)
                    {
                        yield return name;
                    }
                }
            }
        }
    }
}
