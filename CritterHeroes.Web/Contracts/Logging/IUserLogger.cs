using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Models.Logging;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface IUserLogger
    {
        Task<IEnumerable<UserLog>> GetUserLogAsync(DateTime dateFrom, DateTime dateTo);
        Task LogActionAsync(UserActions userAction, string userName);
        Task LogActionAsync<T>(UserActions userAction, string userName, T additionalData);
    }
}
