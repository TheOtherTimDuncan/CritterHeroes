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
            ConfirmEmailEmailCommand command = new ConfirmEmailEmailCommand("to@to.com");
            command.UrlConfirm = "urlconfirm";
            command.Token = "token";
            command.TokenLifespan = TimeSpan.FromHours(4);

            ConfirmEmailEmailBuilder builder = new ConfirmEmailEmailBuilder(mockUrlGenerator.Object, mockOrganizationStateManager.Object, mockLogoService.Object, mockEmailConfiguration.Object);
            EmailMessage message = builder.BuildEmail(command);

            VerifyEmailMessage(command, message, "ConfirmEmail");
        }
    }
}
