using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AR.Azure;
using AR.Domain.Contracts;
using AR.RescueGroups;
using TOTD.Utility.Misc;

namespace AR.Website.Utility
{
    public class StorageSource : Enumeration<StorageSource>
    {
        public static readonly StorageSource Azure = new StorageSource(0, "Azure", () =>
        {
            return new AzureStorage("fflah");
        });

        public static readonly StorageSource RescueGroups = new StorageSource(1, "Rescue Groups", () =>
        {
            return new RescueGroupsStorage();
        });

        private StorageSource()
        {
        }

        private StorageSource(int value, string displayName, Func<IStorageContext> getStorageContext)
            :base(value, displayName)
        {
            this.GetStorageContext = getStorageContext;
        }

        private Func<IStorageContext> GetStorageContext;

        public IStorageContext StorageContext
        {
            get
            {
                return GetStorageContext();
            }
        }

        public string Title
        {
            get
            {
                return DisplayName;
            }
        }
    }
}