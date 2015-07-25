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
    public class UserStore : IAppUserStore
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

        public async Task CreateAsync(AzureAppUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            DynamicTableEntity entity = userMapping.ToStorage(user);
            TableOperation operation = TableOperation.Insert(entity);
            await ExecuteTableOperation(operation).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task DeleteAsync(AzureAppUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            DynamicTableEntity entity = userMapping.ToStorage(user);
            entity.ETag = "*";
            TableOperation operation = TableOperation.Delete(entity);
            await ExecuteTableOperation(operation).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task UpdateAsync(AzureAppUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            DynamicTableEntity entity = userMapping.ToStorage(user);
            entity.ETag = "*";
            TableOperation operation = TableOperation.Replace(entity);
            await ExecuteTableOperation(operation).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<AzureAppUser> FindByIdAsync(string userId)
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

        public async Task<AzureAppUser> FindByNameAsync(string userName)
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

        public Task<string> GetPasswordHashAsync(AzureAppUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(AzureAppUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(AzureAppUser user, string passwordHash)
        {
            ThrowIf.Argument.IsNull(user, "user");

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public async Task<AzureAppUser> FindByEmailAsync(string email)
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

        public Task<string> GetEmailAsync(AzureAppUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AzureAppUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            return Task.FromResult(user.IsEmailConfirmed);
        }

        public Task SetEmailAsync(AzureAppUser user, string email)
        {
            ThrowIf.Argument.IsNull(user, "user");

            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(AzureAppUser user, bool isConfirmed)
        {
            ThrowIf.Argument.IsNull(user, "user");

            user.IsEmailConfirmed = isConfirmed;
            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(AzureAppUser user, string roleName)
        {
            IdentityRole role = IdentityRole.All.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
            {
                throw new ArgumentOutOfRangeException(roleName);
            }
            user.AddRole(role);
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(AzureAppUser user)
        {
            ThrowIf.Argument.IsNull(user, "user");

            IList<string> result = user.Roles.Select(x => x.Name).ToList();
            return Task.FromResult(result);
        }

        public Task<bool> IsInRoleAsync(AzureAppUser user, string roleName)
        {
            bool result = user.Roles.Any(x => x.Name == roleName);
            return Task.FromResult(result);
        }

        public Task RemoveFromRoleAsync(AzureAppUser user, string roleName)
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

        public Task<int> GetAccessFailedCountAsync(AzureAppUser user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(AzureAppUser user)
        {
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(AzureAppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(AzureAppUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(AzureAppUser user)
        {
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(AzureAppUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(AzureAppUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(AzureAppUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetTwoFactorEnabledAsync(AzureAppUser user, bool enabled)
        {
            throw new NotImplementedException();
        }
    }
}
