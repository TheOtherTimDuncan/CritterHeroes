using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Models.Data;
using Microsoft.WindowsAzure.Storage.Table;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Data
{
    public class AnimalStatusAzureStorageContext : BaseDataAzureStorageContext<AnimalStatus>
    {
        public AnimalStatusAzureStorageContext(IAzureConfiguration azureConfiguration)
            : base(azureConfiguration)
        {
        }

        public override AnimalStatus FromStorage(DynamicTableEntity tableEntity)
        {
            return new AnimalStatus(tableEntity["ID"].StringValue, tableEntity["Name"].StringValue, tableEntity["Description"].StringValue);
        }

        public override DynamicTableEntity ToStorage(AnimalStatus entity)
        {
            DynamicTableEntity tableEntity = base.ToStorage(entity);

            tableEntity["ID"] = new EntityProperty(entity.ID);
            tableEntity["Name"] = new EntityProperty(entity.Name);
            tableEntity["Description"] = new EntityProperty(entity.Description);

            return tableEntity;
        }

        protected override string GetRowKey(AnimalStatus entity)
        {
            return entity.ID;
        }
    }
}
