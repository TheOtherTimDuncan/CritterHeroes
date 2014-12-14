using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Models.Logging;

namespace CH.Domain.Contracts.Logging
{
    public interface IEmailLogger
    {
        Task<IEnumerable<EmailLog>> GetEmailLogAsync(DateTime dateFrom, DateTime dateTo);
        Task LogEmailAsync(EmailLog emailLog);
    }
}
