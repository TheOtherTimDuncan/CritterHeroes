using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace AR.Azure.Storage
{
    public abstract class StorageEntity<T> where T : class
    {
        private T _entity;
        private DynamicTableEntity _tableEntity;

        public StorageEntity()
        {
        }

        public T Entity
        {
            get
            {
                return _entity;
            }
            set
            {
                _entity = value;
                _tableEntity = new DynamicTableEntity();
                if (value != null)
                {
                    TableEntity.PartitionKey = this.PartitionKey;
                    TableEntity.RowKey = this.RowKey;
                    CopyEntityToStorage(_tableEntity, value);
                }
            }
        }

        public DynamicTableEntity TableEntity
        {
            get
            {
                return _tableEntity;
            }
            set
            {
                _tableEntity = value;
                if (value != null)
                {
                    _entity = CreateEntityFromStorage(value);
                }
                else
                {
                    _entity = null;
                }
            }
        }

        public virtual string PartitionKey
        {
            get
            {
                return typeof(T).Name;
            }
        }

        public abstract string RowKey
        {
            get;
        }

        protected abstract T CreateEntityFromStorage(DynamicTableEntity tableEntity);
        protected abstract void CopyEntityToStorage(DynamicTableEntity tableEntity, T entity);
    }
}
