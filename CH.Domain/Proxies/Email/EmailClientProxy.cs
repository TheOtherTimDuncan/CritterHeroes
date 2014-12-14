using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts.Email;
using CH.Domain.Contracts.Logging;
using CH.Domain.Models;
using CH.Domain.Models.Logging;
using SendGrid;

namespace CH.Domain.Proxies.Email
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
            Web transport = new Web(credentials);
            await transport.DeliverAsync(message);
        }
    }
}
