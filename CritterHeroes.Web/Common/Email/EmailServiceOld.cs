using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Email;
using SimpleInjector;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Common.Email
{
    public class EmailServiceOld : IEmailService
    {
        private Container _container;

        public EmailServiceOld(Container container)
        {
            ThrowIf.Argument.IsNull(container, "container");
            this._container = container;
        }

        public async Task<CommandResult> SendEmailAsync<TParameter>(TParameter command) where TParameter : EmailCommand
        {
            return await _container.GetInstance<IEmailHandler<TParameter>>().ExecuteAsync(command);
        }
    }
}