using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Handlers.Emails;
using CH.Domain.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.EmailTests
{
    [TestClass]
    public class EmailBuilderTests
    {
        [TestMethod]
        public void CreatesHtmlAndTextBody()
        {
            EmailMessage message = new EmailMessage();
            EmailBuilder
                .Begin(message)
                .AddParagraph("text")
                .End();
            message.TextBody.Should().Be("text\r\n");
            message.HtmlBody.Should().Be("<html><body><p style='font-family: \"Open Sans\", \"Helvetica Neue\", Helvetica, Arial, sans-serif;'>text</p></body></html>");
        }
    }
}
