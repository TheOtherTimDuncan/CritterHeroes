using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IAzureStorageContext<T> : IStorageContext<T> where T : class
    {
    }
}
