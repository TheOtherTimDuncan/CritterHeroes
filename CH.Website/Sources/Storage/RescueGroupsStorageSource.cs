using System;
using System.Collections.Generic;
using System.Linq;
using CH.Dependency;
using CH.Domain.Contracts;
using CH.RescueGroups;

namespace CH.Website.Sources.Storage
{
    public class RescueGroupsStorageSource : IStorageSource
    {
        private IStorageContext _storageContext;

        public RescueGroupsStorageSource()
        {
            this._storageContext = DependencyContainer.Using<RescueGroupsStorage>();
        }

        public int ID
        {
            get
            {
                return 1;
            }
        }

        public IStorageContext StorageContext
        {
            get
            {
                return _storageContext;
            }
        }

        public string Title
        {
            get
            {
                return "Rescue Groups";
            }
        }
    }
}