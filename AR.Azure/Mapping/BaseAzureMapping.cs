using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AR.Azure.Mapping
{
    public abstract class BaseAzureMapping<T> : IAzureMapping<T> where T : class
    {
        public virtual string PartitionKey
        {
            get
            {
                return typeof(T).Name;
            }
        }

        public abstract DynamicTableEntity ToEntity(T model);
        public abstract T ToModel(DynamicTableEntity entity);

        public virtual IEnumerable<DynamicTableEntity> ToEntity(IEnumerable<T> models)
        {
            foreach (T model in models)
            {
                yield return ToEntity(model);
            }
        }

        public virtual IEnumerable<T> ToModel(IEnumerable<DynamicTableEntity> entities)
        {
            foreach (DynamicTableEntity entity in entities)
            {
                yield return ToModel(entity);
            }
        }
    }
}
