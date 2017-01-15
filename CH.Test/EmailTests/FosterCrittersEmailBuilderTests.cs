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
    public class FosterCrittersEmailBuilderTests : EmailBuilderBaseTest
    {
        [TestMethod]
        public void GeneratesCorrectHtml()
        {
            FosterCrittersEmailCommand command = CreateTestCommand<FosterCrittersEmailCommand, FosterCrittersEmailCommand.FosterCrittersEmailData>();

            var critter1 = new FosterCrittersEmailCommand.Critter()
            {
                Name = "Name1",
                RescueGroupsID = 1,
                RescueID = "rescueid1",
                Sex = "sex1",
                Status = "status1",
                ThumbnailUrl = "thumbnail1"
            };

            var critter2 = new FosterCrittersEmailCommand.Critter()
            {
                Name = "Name2",
                RescueGroupsID = 2,
                RescueID = "rescueid2",
                Sex = "sex2",
                Status = "status2",
                ThumbnailUrl = "thumbnail2"
            };

            command.EmailData.Critters = new[] { critter1, critter2 };

            FosterCrittersEmailBuilder builder = new FosterCrittersEmailBuilder();
            EmailMessage message = builder.BuildEmail(command);

            WriteEmailMessage(message, "FosterCritters");
        }
    }
}
