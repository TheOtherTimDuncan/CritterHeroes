using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Mailer.Core;

namespace CH.Test.EmailTests
{
    [TestClass ]
    public class ResetPasswordNotificationEmailBuilderTests : EmailBuilderBaseTest
    {
        [TestMethod]
        public void GeneratesCorrectHtml()
        {
            ResetPasswordNotificationEmailCommand command = CreateTestCommand<ResetPasswordNotificationEmailCommand, BaseEmailData>();

            ResetPasswordNotificationEmailBuilder builder = new ResetPasswordNotificationEmailBuilder();
            EmailMessage message = builder.BuildEmail(command);

            WriteEmailMessage(message, "ResetPasswordNotification");
        }
    }
}
