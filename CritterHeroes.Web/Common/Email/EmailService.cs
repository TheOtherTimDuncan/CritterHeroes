using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;

namespace CritterHeroes.Web.Common.Email
{
    public class EmailService : IEmailService
    {
        private IFileSystem _fileSystem;
        private IHttpContext _httpContext;
        private IEmailConfiguration _configuration;

        public EmailService(IFileSystem fileSystem, IHttpContext httpContext, IEmailConfiguration emailConfiguration)
        {
            this._fileSystem = fileSystem;
            this._httpContext = httpContext;
            this._configuration = emailConfiguration;
        }

        public async Task<CommandResult> SendEmailAsync<TParameter>(TParameter command) where TParameter : EmailCommand
        {
            string folder = _httpContext.Server.MapPath($"~/Areas/Emails/{command.EmailName}");

            string filenameSubject = _fileSystem.CombinePath(folder, "Subject.txt");
            string filenameHtmlBody = _fileSystem.CombinePath(folder, "Body.html");
            string filenameTxtBody = _fileSystem.CombinePath(folder, "Body.txt");

            var email = new
            {
                From = command.EmailFrom ?? _configuration.DefaultFrom,
                To = command.EmailTo,
                SubjectTemplate = _fileSystem.ReadAllText(filenameSubject),
                HtmlTemplate = _fileSystem.ReadAllText(filenameHtmlBody),
                TextTemplate = _fileSystem.ReadAllText(filenameTxtBody),
                Data = command.EmailData

            };

            return CommandResult.Success();
        }
    }
}
