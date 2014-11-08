using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models.Data;
using Microsoft.WindowsAzure.Storage.Table;

namespace CH.Azure.Storage.Data
{
    public class BreedStorageEntity : StorageEntity<Breed>
    {
        public override string RowKey
        {
            get
            {
                return Entity.ID;
            }
        }

        protected override void CopyEntityToStorage(DynamicTableEntity tableEntity, Breed entity)
        {
            tableEntity["ID"] = new EntityProperty(entity.ID);
            tableEntity["Species"] = new EntityProperty(entity.Species);
            tableEntity["BreedName"] = new EntityProperty(entity.BreedName);
        }

        protected override Breed CreateEntityFromStorage(DynamicTableEntity tableEntity)
        {
            return new Breed(tableEntity["ID"].StringValue, tableEntity["Species"].StringValue, tableEntity["BreedName"].StringValue);
        }
    }
}
