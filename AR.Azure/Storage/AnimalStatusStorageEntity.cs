using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AR.Domain.Models.Data;
using Microsoft.WindowsAzure.Storage.Table;

namespace AR.Azure.Storage
{
    public class AnimalStatusStorageEntity : StorageEntity<AnimalStatus>
    {
        public override string RowKey
        {
            get
            {
                return Entity.ID;
            }
        }

        protected override void CopyEntityToStorage(DynamicTableEntity tableEntity, AnimalStatus entity)
        {
            tableEntity["ID"] = new EntityProperty(entity.ID);
            tableEntity["Name"] = new EntityProperty(entity.Name);
            tableEntity["Description"] = new EntityProperty(entity.Description);
        }

        protected override AnimalStatus CreateEntityFromStorage(DynamicTableEntity tableEntity)
        {
            return new AnimalStatus(tableEntity["ID"].StringValue, tableEntity["Name"].StringValue, tableEntity["Description"].StringValue);
        }
    }
}
