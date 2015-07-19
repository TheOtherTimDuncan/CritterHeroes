using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.Azure.Utility;
using CritterHeroes.Web.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.Azure.Storage
{
    public class OrganizationAzureStorage : BaseAzureStorageContext<Organization>
    {
        public OrganizationAzureStorage(IAzureConfiguration azureConfiguration)
            : base("organization", azureConfiguration)
        {
        }

        public override Organization FromStorage(DynamicTableEntity tableEntity)
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

        public override DynamicTableEntity ToStorage(Organization entity)
        {
            DynamicTableEntity tableEntity = base.ToStorage(entity);

            tableEntity["ID"] = new EntityProperty(entity.ID);
            tableEntity["FullName"] = new EntityProperty(entity.FullName);
            tableEntity["ShortName"] = new EntityProperty(entity.ShortName);
            tableEntity["AzureName"] = new EntityProperty(entity.AzureName);
            tableEntity["LogoFilename"] = new EntityProperty(entity.LogoFilename);
            tableEntity["EmailAddress"] = new EntityProperty(entity.EmailAddress);
            tableEntity["SupportedCritters"] = new EntityProperty(JsonConvert.SerializeObject(entity.SupportedCritters));

            return tableEntity;
        }

        protected override string GetRowKey(Organization entity)
        {
            return entity.ID.ToString();
        }
    }
}
