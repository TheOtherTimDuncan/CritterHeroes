using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Mailer.Core;

namespace CH.Test.EmailTests
{
    [TestClass]
    public class ConfirmEmailEmailBuilderTests : EmailBuilderBaseTest
    {
        [TestMethod]
        public void GeneratesCorrectHtml()
        {
            ConfirmEmailEmailCommand command = CreateTestCommand<ConfirmEmailEmailCommand, ConfirmEmailEmailCommand.ConfirmEmailData>();
            command.EmailData.UrlConfirm = "urlconfirm";
            command.EmailData.Token = "token";
            command.EmailData.TokenLifespan = TimeSpan.FromHours(4);

            ConfirmEmailEmailBuilder builder = new ConfirmEmailEmailBuilder();
            EmailMessage message = builder.BuildEmail(command);

            WriteEmailMessage(message, "ConfirmEmail");
        }
    }
}
