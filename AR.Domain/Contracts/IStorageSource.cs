using System;
using System.Collections.Generic;

namespace AR.Domain.Contracts
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
