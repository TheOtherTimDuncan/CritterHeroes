using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Models.Logging;

namespace CH.Domain.Contracts.Logging
{
    public interface IUserLogger
    {
        Task<IEnumerable<UserLog>> GetUserLog(DateTime dateFrom, DateTime dateTo);
        Task LogAction(UserActions userAction, string userName);
        Task LogAction<T>(UserActions userAction, string userName, T additionalData);
    }
}
