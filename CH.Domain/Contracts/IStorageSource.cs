using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts
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

        IStorageContext StorageContext
        {
            get;
        }
    }
}
