using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Models.Data;
using Microsoft.WindowsAzure.Storage.Table;

namespace AR.Azure.Mapping
{
    public class AnimalStatusMapping : BaseAzureMapping<AnimalStatus>
    {
        public override DynamicTableEntity ToEntity(AnimalStatus model)
        {
            DynamicTableEntity entity = new DynamicTableEntity(PartitionKey, model.Name);
            entity["Name"] = new EntityProperty(model.Name);
            entity["Description"] = new EntityProperty(model.Description);
            return entity;
        }

        public override AnimalStatus ToModel(DynamicTableEntity entity)
        {
            return new AnimalStatus(entity["Name"].StringValue, entity["Description"].StringValue);
        }
    }
}
