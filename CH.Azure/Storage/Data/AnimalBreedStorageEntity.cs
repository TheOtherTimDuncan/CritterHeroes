using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models.Data;
using Microsoft.WindowsAzure.Storage.Table;

namespace CH.Azure.Storage.Data
{
    public class AnimalBreedStorageEntity : StorageEntity<AnimalBreed>
    {
        public override string RowKey
        {
            get
            {
                return Entity.ID;
            }
        }

        protected override void CopyEntityToStorage(DynamicTableEntity tableEntity, AnimalBreed entity)
        {
            tableEntity["ID"] = new EntityProperty(entity.ID);
            tableEntity["Species"] = new EntityProperty(entity.Species);
            tableEntity["BreedName"] = new EntityProperty(entity.BreedName);
        }

        protected override AnimalBreed CreateEntityFromStorage(DynamicTableEntity tableEntity)
        {
            return new AnimalBreed(tableEntity["ID"].StringValue, tableEntity["Species"].StringValue, tableEntity["BreedName"].StringValue);
        }
    }
}
