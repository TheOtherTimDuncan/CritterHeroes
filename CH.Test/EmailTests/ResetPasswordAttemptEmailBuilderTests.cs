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
    public class ResetPasswordAttemptEmailBuilderTests : EmailBuilderBaseTest
    {
        [TestMethod]
        public void GeneratesCorrectHtml()
        {
            ResetPasswordAttemptEmailCommand command = new ResetPasswordAttemptEmailCommand("to@to.com");

            ResetPasswordAttemptEmailBuilder builder = new ResetPasswordAttemptEmailBuilder(mockUrlGenerator.Object, mockOrganizationStateManager.Object, mockLogoService.Object, mockEmailConfiguration.Object);
            EmailMessage message = builder.BuildEmail(command);

            VerifyEmailMessage(command, message, "ResetPasswordAttempt");
        }
    }
}
