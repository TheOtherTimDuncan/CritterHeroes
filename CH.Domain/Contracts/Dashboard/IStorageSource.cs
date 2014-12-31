using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts.Dashboard
{
    public interface IStorageSource
    {
        int ID
        {
            get;
        }

        string Title
        {
            get;
        }
    }
}
