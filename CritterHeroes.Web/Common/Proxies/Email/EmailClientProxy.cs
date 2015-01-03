using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using SendGrid;

namespace CritterHeroes.Web.Common.Proxies.Email
{
    public class EmailClientProxy : IEmailClient
    {
        private IEmailConfiguration _configuration;
        private IEmailLogger _logger;

        public EmailClientProxy(IEmailConfiguration configuration, IEmailLogger logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        public async Task SendAsync(EmailMessage emailMessage)
        {
            await SendMessageAsync(emailMessage, null);
        }

        public async Task SendAsync(EmailMessage emailMessage, string forUserID)
        {
            await SendMessageAsync(emailMessage, forUserID);
        }

        private async Task SendMessageAsync(EmailMessage emailMessage, string forUserID)
        {
            EmailLog emailLog = new EmailLog(DateTime.UtcNow, emailMessage)
            {
                ForUserID = forUserID
            };
            await _logger.LogEmailAsync(emailLog);

            SendGridMessage message = new SendGridMessage()
            {
                From = new MailAddress(emailMessage.From ?? _configuration.DefaultFrom),
                Subject = emailMessage.Subject,
                Html = emailMessage.HtmlBody,
                Text = emailMessage.TextBody
            };

            message.AddTo(emailMessage.To);

            NetworkCredential credentials = new NetworkCredential(_configuration.Username, _configuration.Password);
            SendGrid.Web transport = new SendGrid.Web(credentials);
            await transport.DeliverAsync(message);
        }
    }
}
