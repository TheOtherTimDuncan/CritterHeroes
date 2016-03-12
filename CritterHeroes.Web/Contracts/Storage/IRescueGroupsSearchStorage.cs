﻿using System;
using System.Collections.Generic;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IRescueGroupsSearchStorage<T> : IRescueGroupsStorageContext<T> where T : class
    {
        int ResultLimit
        {
            get;
            set;
        }

        IEnumerable<SearchField> Fields
        {
            get;
        }


        IEnumerable<SearchFilter> Filters
        {
            get;
            set;
        }

        string FilterProcessing
        {
            get;
            set;
        }
    }
}
