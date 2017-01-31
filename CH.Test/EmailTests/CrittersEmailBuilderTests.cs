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
    public class CrittersEmailBuilderTests : EmailBuilderBaseTest
    {
        [TestMethod]
        public void GeneratesCorrectHtml()
        {
            CrittersEmailCommand command = new CrittersEmailCommand("to@to.com");

            var critter1 = new CrittersEmailCommand.Critter()
            {
                FosterEmail = "email1",
                FosterName = "foster1",
                Name = "Name1",
                RescueGroupsID = 1,
                RescueID = "rescueid1",
                Sex = "sex1",
                Status = "status1",
                ThumbnailUrl = "thumbnail1",
                IsBirthDateExact = true,
                Birthdate = DateTime.Now.AddDays(-1).AddYears(-2)
            };

            var critter2 = new CrittersEmailCommand.Critter()
            {
                Location = "location2",
                Name = "Name2",
                RescueGroupsID = 2,
                RescueID = "rescueid2",
                Sex = "sex2",
                Status = "status2",
                ThumbnailUrl = "thumbnail2",
                IsBirthDateExact = false,
                Birthdate = DateTime.Now.AddDays(1).AddYears(-1)
            };

            command.Critters = new[] { critter1, critter2 };

            CrittersEmailBuilder builder = new CrittersEmailBuilder(mockUrlGenerator.Object, mockOrganizationStateManager.Object, mockLogoService.Object, mockEmailConfiguration.Object);
            EmailMessage message = builder.BuildEmail(command);

            VerifyEmailMessage(command, message, "Critters");
        }
    }
}
