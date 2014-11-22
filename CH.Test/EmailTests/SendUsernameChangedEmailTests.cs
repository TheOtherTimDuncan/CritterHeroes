using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Handlers.Emails;
using CH.Domain.Models;
using CH.Domain.Proxies.Email;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.EmailTests
{
    [TestClass]
    public class SendUsernameChangedEmailTests
    {
        //[TestMethod]
        public async Task CanSendEmailWithSendGrid()
        {
            EmailConfiguration configuration = new EmailConfiguration();
            EmailClientProxy client = new EmailClientProxy(configuration);
            SendUsernameChangedEmail handler = new SendUsernameChangedEmail(client);
            await handler.Execute("tduncan72@gmail.com", "old", "new", "FFLAH");
        }
    }
}
