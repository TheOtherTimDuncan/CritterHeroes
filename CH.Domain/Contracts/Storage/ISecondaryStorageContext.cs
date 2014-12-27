using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts.Storage
{
    /// <summary>
    /// Marker interface for storage sources that contain a copy of data from a master data source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISecondaryStorageContext<T> : IStorageContext<T> where T : class
    {
    }
}
