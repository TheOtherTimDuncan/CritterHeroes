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
    public class FosterSummaryEmailBuilderTests : EmailBuilderBaseTest
    {
        [TestMethod]
        public void GeneratesCorrectHtml()
        {
            FosterSummaryEmailCommand command = CreateTestCommand<FosterSummaryEmailCommand, FosterSummaryEmailCommand.FosterSummaryEmailData>();

            var summary1 = new FosterSummaryEmailCommand.Summary()
            {
                FosterName = "summary1",
                BabyCount = 1,
                YoungCount = 2,
                AdultCount = 3,
                SeniorCount = 4
            };

            var summary2 = new FosterSummaryEmailCommand.Summary()
            {
                FosterName = "summary2",
                BabyCount = 5,
                YoungCount = 6,
                AdultCount = 7,
                SeniorCount = 8
            };

            command.EmailData.Critters = new[] { summary1, summary2 };

            FosterSummaryEmailBuilder builder = new FosterSummaryEmailBuilder();
            EmailMessage message = builder.BuildEmail(command);

            WriteEmailMessage(message, "FosterSummary");
        }
    }
}
