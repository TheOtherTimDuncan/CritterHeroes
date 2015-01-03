using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Common.Handlers.Email
{
    public class SendUsernameChangedEmail 
    {
        private IEmailClient _emailClient;

        public SendUsernameChangedEmail(IEmailClient emailClient)
            : base()
        {
            _emailClient = emailClient;
        }

        public async Task Execute(string email, string oldUsername, string newUsername, string organizationName)
        {
            EmailMessage message = new EmailMessage()
            {
                Subject = organizationName + " Admin Notification"
            };

            message.To.Add(email);

            EmailBuilder
                .Begin(message)
                .AddParagraph(string.Format("This is a notification that your username has been changed from {0} to {1}.", oldUsername, newUsername))
                .End();

            await _emailClient.SendAsync(message);
        }
    }
}
