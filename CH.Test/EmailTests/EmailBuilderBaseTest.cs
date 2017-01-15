using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Shared.Commands;
using FluentAssertions;
using Newtonsoft.Json;
using TOTD.Mailer.Core;
using TOTD.Utility.UnitTestHelpers;

namespace CH.Test.EmailTests
{
    public class EmailBuilderBaseTest
    {
        public TCommand CreateTestCommand<TCommand, TEmailData>()
            where TCommand : EmailCommand<TEmailData>
            where TEmailData : BaseEmailData, new()
        {
            TCommand command = (TCommand)Activator.CreateInstance(typeof(TCommand), "to@to.com");

            command.EmailFrom = "from@from.com";
            command.EmailData.UrlHome = "urlhome";
            command.EmailData.UrlLogo = "http://127.0.0.1:10000/devstoreaccount1/fflah/logo%20-%20optimized.svg";
            command.EmailData.OrganizationFullName = "orgfullname";

            return command;
        }

        public void WriteEmailMessage(EmailMessage emailMessage, string emailTitle)
        {
            emailMessage.Subject.Should().NotBeNullOrEmpty();

            string folder = Path.Combine(UnitTestHelper.GetSolutionRoot(), "TestResults");
            File.WriteAllText(Path.Combine(folder, $"{emailTitle}.html"), emailMessage.HtmlBody);
            File.WriteAllText(Path.Combine(folder, $"{emailTitle}.txt"), emailMessage.TextBody);
            File.WriteAllText(Path.Combine(folder, $"{emailTitle}.json"), JsonConvert.SerializeObject(emailMessage, Formatting.Indented));
        }
    }
}
