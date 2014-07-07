using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOTD.Utility.EnumerableHelpers;

namespace AR.Domain.Models.Status
{
    public class StorageItem
    {
        public int StorageID
        {
            get;
            set;
        }

        public IEnumerable<DataItem> Items
        {
            get;
            set;
        }

        public int ValidCount
        {
            get
            {
                return Items.NullSafeWhere(x => x.IsValid).Count();
            }
        }

        public int InvalidCount
        {
            get
            {
                return Items.NullSafeWhere(x => !x.IsValid).Count();
            }
        }
    }
}
