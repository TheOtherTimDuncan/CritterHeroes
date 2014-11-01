using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Models;
using Microsoft.WindowsAzure.Storage.Table;
using TOTD.Utility.Misc;

namespace AR.Azure.Storage
{
    public class OrganizationStorageEntity : StorageEntity<Organization>
    {
        public override string RowKey
        {
            get
            {
                return Entity.ID.ToString();
            }
        }

        protected override void CopyEntityToStorage(DynamicTableEntity tableEntity, Organization entity)
        {
            tableEntity["ID"] = new EntityProperty(entity.ID);
            tableEntity["FullName"] = new EntityProperty(entity.FullName);
            tableEntity["ShortName"] = new EntityProperty(entity.ShortName);
            tableEntity["AzureTableName"] = new EntityProperty(entity.AzureTableName);
            tableEntity["SupportedCritters"] = new EntityProperty(string.Join("|", entity.SupportedCritters));
        }

        protected override Organization CreateEntityFromStorage(DynamicTableEntity tableEntity)
        {
            Guid? organizationID = tableEntity["ID"].GuidValue;
            if (organizationID == null)
            {
                throw new AzureException("Organization missing ID: " + tableEntity.RowKey);
            }

            return new Organization(organizationID.Value)
            {
                FullName = tableEntity["FullName"].StringValue,
                ShortName = tableEntity["ShortName"].StringValue,
                AzureTableName = tableEntity["AzureTableName"].StringValue,
                SupportedCritters = tableEntity["SupportedCritters"].StringValue.IfNotNull(x => x.Split('|'))
            };
        }
    }
}
