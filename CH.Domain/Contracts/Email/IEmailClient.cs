using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Models;

namespace CH.Domain.Contracts.Email
{
    public interface IEmailClient
    {
        Task SendAsync(EmailMessage emailMessage);
        Task SendAsync(EmailMessage emailMessage, string forUserID);
    }
}
