using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Contracts.Email
{
    public interface IEmailClient
    {
        Task SendAsync(EmailMessage emailMessage);
        Task SendAsync(EmailMessage emailMessage, string forUserID);
    }
}
