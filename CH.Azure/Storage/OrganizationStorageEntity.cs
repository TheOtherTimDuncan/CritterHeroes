using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Azure.Utility;
using CH.Domain.Models;
using CH.Domain.Models.Data;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using TOTD.Utility.Misc;

namespace CH.Azure.Storage
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
            tableEntity["AzureName"] = new EntityProperty(entity.AzureName);
            tableEntity["LogoFilename"] = new EntityProperty(entity.LogoFilename);
            tableEntity["EmailAddress"] = new EntityProperty(entity.EmailAddress);
            tableEntity["SupportedCritters"] = new EntityProperty(JsonConvert.SerializeObject(entity.SupportedCritters));
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
                AzureName = tableEntity["AzureName"].StringValue,
                LogoFilename = tableEntity["LogoFilename"].StringValue,
                EmailAddress = tableEntity["EmailAddress"].StringValue,
                SupportedCritters = JsonConvert.DeserializeObject<IEnumerable<Species>>(tableEntity["SupportedCritters"].StringValue)
            };
        }
    }
}
