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
    public class ResetPasswordEmailBuilderTests : EmailBuilderBaseTest
    {
        [TestMethod]
        public void GeneratesCorrectHtml()
        {
            ResetPasswordEmailCommand command = CreateTestCommand<ResetPasswordEmailCommand, ResetPasswordEmailCommand.ResetPasswordEmailData>();
            command.EmailData.UrlReset = "urlreset";
            command.EmailData.Token = "token";
            command.EmailData.TokenLifespan = TimeSpan.FromHours(4);

            ResetPasswordEmailBuilder builder = new ResetPasswordEmailBuilder();
            EmailMessage message = builder.BuildEmail(command);

            WriteEmailMessage(message, "ResetPassword");
        }
    }
}
