using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AR.Azure.Mapping
{
    public interface IAzureMapping<T> where T : class
    {
        string PartitionKey
        {
            get;
        }

        DynamicTableEntity ToEntity(T model);
        IEnumerable<DynamicTableEntity> ToEntity(IEnumerable<T> models);

        T ToModel(DynamicTableEntity entity);
        IEnumerable<T> ToModel(IEnumerable<DynamicTableEntity> entities);
    }
}
