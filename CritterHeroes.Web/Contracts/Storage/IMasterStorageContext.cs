using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Storage
{
    /// <summary>
    /// Marker interface for the storage sources that contain the master copy of a data source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMasterStorageContext<T> : IStorageContext<T> where T : class
    {
    }
}
