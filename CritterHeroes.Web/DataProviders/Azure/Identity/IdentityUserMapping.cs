using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.Azure.Utility;
using CritterHeroes.Web.Common.Identity;
using Microsoft.WindowsAzure.Storage.Table;

namespace CritterHeroes.Web.DataProviders.Azure.Identity
{
    public class IdentityUserMapping
    {
        public DynamicTableEntity ToStorage(AppUser user)
        {
            DynamicTableEntity entity = new DynamicTableEntity(user.Id, user.Id);
            entity["UserName"] = new EntityProperty(user.UserName);
            entity["PasswordHash"] = new EntityProperty(user.PasswordHash);
            entity["Email"] = new EntityProperty(user.Email);
            entity["NewEmail"] = new EntityProperty(user.NewEmail);
            entity["IsEmailConfirmed"] = new EntityProperty(user.IsEmailConfirmed);
            entity["FirstName"] = new EntityProperty(user.FirstName);
            entity["LastName"] = new EntityProperty(user.LastName);

            string roles = user.Roles.Any() ? string.Join(",", user.Roles.Select(x => x.ID)) : null;
            entity["Roles"] = new EntityProperty(roles);

            return entity;
        }

        public AppUser FromStorage(DynamicTableEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            AppUser user = new AppUser(entity.PartitionKey, entity["UserName"].StringValue);

            user.PasswordHash = entity["PasswordHash"].StringValue;
            user.Email = entity["Email"].StringValue;
            user.NewEmail = entity.SafeGetEntityPropertyStringValue("NewEmail");
            user.IsEmailConfirmed = entity["IsEmailConfirmed"].BooleanValue ?? false;
            user.FirstName = entity.SafeGetEntityPropertyStringValue("FirstName");
            user.LastName = entity.SafeGetEntityPropertyStringValue("LastName");

            string roles = entity.SafeGetEntityPropertyStringValue("Roles");
            if (roles != null)
            {
                IEnumerable<string> roleIDs = roles.Split(',');
                foreach (IdentityRole role in IdentityRole.All)
                {
                    if (roleIDs.Any(x => x == role.ID))
                    {
                        user.AddRole(role);
                    }
                }
            }

            return user;
        }
    }
}
