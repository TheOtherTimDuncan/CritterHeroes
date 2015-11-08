using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IEmailStorageService
    {
        Task<string> GetEmailAsync(Guid emailID);
        Task SaveEmailAsync(Guid emailID, string emailData);
    }
}
