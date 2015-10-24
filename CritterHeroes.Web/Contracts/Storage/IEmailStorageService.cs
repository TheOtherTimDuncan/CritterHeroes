using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IEmailStorageService
    {
        Task<EmailMessage> GetEmailAsync(Guid emailID);
        Task SaveEmailAsync(EmailMessage message, Guid emailID);
    }
}
