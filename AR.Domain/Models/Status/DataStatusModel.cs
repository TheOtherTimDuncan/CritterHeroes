using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR.Domain.Models.Status
{
    public class DataStatusModel
    {
        public IEnumerable<StorageItem> Items
        {
            get;
            set;
        }

        public int DataItemCount
        {
            get;
            set;
        }
    }
}
