using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Identity;
using Microsoft.WindowsAzure.Storage.Table;

namespace AR.Azure.Identity
{
    public class IdentityUserMapping
    {
        public DynamicTableEntity ToStorage(IdentityUser user)
        {
            DynamicTableEntity entity = new DynamicTableEntity(user.Id, user.Id);
            entity["UserName"] = new EntityProperty(user.UserName);
            entity["PasswordHash"] = new EntityProperty(user.PasswordHash);
            entity["Email"] = new EntityProperty(user.Email);
            entity["IsEmailConfirmed"] = new EntityProperty(user.IsEmailConfirmed);

            string roles = user.Roles.Any() ? string.Join(",", user.Roles.Select(x => x.ID)) : null;
            entity["Roles"] = new EntityProperty(roles);

            return entity;
        }

        public IdentityUser FromStorage(DynamicTableEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            IdentityUser user = new IdentityUser(entity.PartitionKey, entity["UserName"].StringValue);

            user.PasswordHash = entity["PasswordHash"].StringValue;
            //user.Email = entity["Email"].StringValue;
            user.IsEmailConfirmed = entity["IsEmailConfirmed"].BooleanValue ?? false;

            EntityProperty property;
            if (entity.Properties.TryGetValue("Email", out property))
            {
                user.Email = property.StringValue;
            }

            string roles = entity["Roles"].StringValue;
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
