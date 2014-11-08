using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CH.Azure.Identity
{
    public class UserStore :
        IUserStore<IdentityUser>,
        IUserPasswordStore<IdentityUser>,
        IUserEmailStore<IdentityUser>,
        IUserRoleStore<IdentityUser>
    {
        private IdentityUserMapping userMapping;
        private CloudStorageAccount _account;

        public UserStore(string connectionString)
            : this(connectionString, "user")
        {
        }

        public UserStore(string connectionString, string tableName)
        {
            ThrowIf.Argument.IsNullOrEmpty(connectionString, "connectionString");
            ThrowIf.Argument.IsNullOrEmpty(tableName, "tableName");

            this._account = CloudStorageAccount.Parse(connectionString);
            this.TableName = tableName;

            this.userMapping = new IdentityUserMapping();
        }

        protected string TableName
        {
            get;
            private set;
        }

        public async Task CreateAsync(IdentityUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            DynamicTableEntity entity = userMapping.ToStorage(user);
            TableOperation operation = TableOperation.Insert(entity);
            await ExecuteTableOperation(operation);
        }

        public async Task DeleteAsync(IdentityUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            DynamicTableEntity entity = userMapping.ToStorage(user);
            entity.ETag = "*";
            TableOperation operation = TableOperation.Delete(entity);
            await ExecuteTableOperation(operation);
        }

        public async Task UpdateAsync(IdentityUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            DynamicTableEntity entity = userMapping.ToStorage(user);
            entity.ETag = "*";
            TableOperation operation = TableOperation.Replace(entity);
            await ExecuteTableOperation(operation);
        }

        public async Task<IdentityUser> FindByIdAsync(string userId)
        {
            CloudTable table = await GetCloudTableAsync();
            DynamicTableEntity entity =
            (
                from x in table.CreateQuery<DynamicTableEntity>()
                where x.PartitionKey == userId
                select x
            ).FirstOrDefault();
            return userMapping.FromStorage(entity);
        }

        public async Task<IdentityUser> FindByNameAsync(string userName)
        {
            CloudTable table = await GetCloudTableAsync();
            DynamicTableEntity entity =
            (
                from x in table.CreateQuery<DynamicTableEntity>()
                where x.Properties["UserName"].StringValue == userName
                select x
            ).FirstOrDefault();
            return userMapping.FromStorage(entity);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            ThrowIf.Argument.IsNull(user, "user");

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            if (email.IsNullOrWhiteSpace())
            {
                return null;
            }

            CloudTable table = await GetCloudTableAsync();
            DynamicTableEntity entity =
            (
                from x in table.CreateQuery<DynamicTableEntity>()
                where x.Properties["Email"].StringValue == email
                select x
            ).FirstOrDefault();
            return userMapping.FromStorage(entity);
        }

        public Task<string> GetEmailAsync(IdentityUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            return Task.FromResult(user.IsEmailConfirmed);
        }

        public Task SetEmailAsync(IdentityUser user, string email)
        {
            ThrowIf.Argument.IsNull(user, "user");

            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool isConfirmed)
        {
            ThrowIf.Argument.IsNull(user, "user");

            user.IsEmailConfirmed = isConfirmed;
            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            IdentityRole role = IdentityRole.All.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
            {
                throw new ArgumentOutOfRangeException(roleName);
            }
            user.AddRole(role);
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            IList<string> result = user.Roles.Select(x => x.Name).ToList();
            return Task.FromResult(result);
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            bool result = user.Roles.Any(x => x.Name == roleName);
            return Task.FromResult(result);
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            IdentityRole role = user.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role != null)
            {
                user.RemoveRole(role);
            }
            return Task.FromResult(0);
        }

        public void Dispose()
        {
        }

        protected virtual async Task<CloudTable> GetCloudTableAsync()
        {
            CloudTableClient client = _account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(TableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        protected virtual async Task ExecuteTableOperation(TableOperation tableOperation)
        {
            CloudTable table = await GetCloudTableAsync();
            await table.ExecuteAsync(tableOperation);
        }
    }
}
