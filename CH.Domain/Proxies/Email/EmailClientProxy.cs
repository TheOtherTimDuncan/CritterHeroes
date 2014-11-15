using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts.Email;
using CH.Domain.Models;
using SendGrid;

namespace CH.Domain.Proxies.Email
{
    public class EmailClientProxy : IEmailClient
    {
        private IEmailConfiguration _configuration;

        public EmailClientProxy(IEmailConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task SendAsync(EmailMessage emailMessage)
        {
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
