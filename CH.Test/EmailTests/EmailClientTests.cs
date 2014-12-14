using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models;
using CH.Domain.Proxies.Email;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.EmailTests
{
    [TestClass]
    public class EmailClientTests
    {
        //[TestMethod]
        //public async Task CanSendEmailWithSendGrid()
        //{
        //    EmailConfiguration configuration = new EmailConfiguration();
        //    EmailClientProxy client = new EmailClientProxy(configuration);
        //    EmailMessage message = new EmailMessage()
        //    {
        //        From = "tduncan72@gmail.com",
        //        Subject = "Test",
        //        HtmlBody = "<p>Test</p>",
        //        TextBody = "Test"
        //    };
        //    message.To.Add("tduncan72@gmail.com");
        //    await client.SendAsync(message);
        //}
    }
}
