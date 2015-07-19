using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Data.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Data
{
    public class BreedAzureStorageContext : BaseDataAzureStorageContext<Breed>
    {
        public BreedAzureStorageContext(IAzureConfiguration azureConfiguration)
            : base(azureConfiguration)
        {
        }

        public override Breed FromStorage(DynamicTableEntity tableEntity)
        {
            return new Breed(tableEntity["ID"].StringValue, tableEntity["Species"].StringValue, tableEntity["BreedName"].StringValue);
        }

        public override DynamicTableEntity ToStorage(Breed entity)
        {
            DynamicTableEntity tableEntity = base.ToStorage(entity);

            tableEntity["ID"] = new EntityProperty(entity.ID);
            tableEntity["Species"] = new EntityProperty(entity.Species);
            tableEntity["BreedName"] = new EntityProperty(entity.BreedName);

            return tableEntity;
        }

        protected override string GetRowKey(Breed entity)
        {
            return entity.ID;
        }
    }
}
