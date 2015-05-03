using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Common.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.Azure.Identity
{
    public class UserStore : IApplicationUserStore
    {
        private IdentityUserMapping userMapping;
        private CloudStorageAccount _account;

        public UserStore(IAzureConfiguration azureConfiguration)
        {
            ThrowIf.Argument.IsNull(azureConfiguration, "azureConfiguration");

            this._account = CloudStorageAccount.Parse(azureConfiguration.ConnectionString);
            this.TableName = "user";

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
            await ExecuteTableOperation(operation).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task DeleteAsync(IdentityUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            DynamicTableEntity entity = userMapping.ToStorage(user);
            entity.ETag = "*";
            TableOperation operation = TableOperation.Delete(entity);
            await ExecuteTableOperation(operation).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task UpdateAsync(IdentityUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            DynamicTableEntity entity = userMapping.ToStorage(user);
            entity.ETag = "*";
            TableOperation operation = TableOperation.Replace(entity);
            await ExecuteTableOperation(operation).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<IdentityUser> FindByIdAsync(string userId)
        {
            CloudTable table = await GetCloudTableAsync().ConfigureAwait(continueOnCapturedContext: false);
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
            CloudTable table = await GetCloudTableAsync().ConfigureAwait(continueOnCapturedContext: false);
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

            CloudTable table = await GetCloudTableAsync().ConfigureAwait(continueOnCapturedContext: false);
            DynamicTableEntity entity =
            (
                from x in table.CreateQuery<DynamicTableEntity>()
                where x.Properties["Email"].StringValue == email || x.Properties["NewEmail"].StringValue == email
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
            await table.CreateIfNotExistsAsync().ConfigureAwait(continueOnCapturedContext: false);
            return table;
        }

        protected virtual async Task ExecuteTableOperation(TableOperation tableOperation)
        {
            CloudTable table = await GetCloudTableAsync().ConfigureAwait(continueOnCapturedContext: false);
            await table.ExecuteAsync(tableOperation).ConfigureAwait(continueOnCapturedContext: false);
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            throw new NotImplementedException();
        }
    }
}
