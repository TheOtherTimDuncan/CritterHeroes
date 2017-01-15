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
            ResetPasswordEmailCommand command = new ResetPasswordEmailCommand("to@to.com");
            command.UrlReset = "urlreset";
            command.Token = "token";
            command.TokenLifespan = TimeSpan.FromHours(4);

            ResetPasswordEmailBuilder builder = new ResetPasswordEmailBuilder(mockUrlGenerator.Object, mockOrganizationStateManager.Object, mockLogoService.Object, mockEmailConfiguration.Object);
            EmailMessage message = builder.BuildEmail(command);

            VerifyEmailMessage(command, message, "ResetPassword");
        }
    }
}
