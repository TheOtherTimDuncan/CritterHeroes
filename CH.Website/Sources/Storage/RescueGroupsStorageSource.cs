using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Dashboard;

namespace CH.Website.Sources.Storage
{
    public class RescueGroupsStorageSource : IStorageSource
    {
        public int ID
        {
            get
            {
                return 1;
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