using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Models.Logging;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface IEmailLogger
    {
        Task<IEnumerable<EmailLog>> GetEmailLogAsync(DateTime dateFrom, DateTime dateTo);
        Task LogEmailAsync(EmailLog emailLog);
    }
}
