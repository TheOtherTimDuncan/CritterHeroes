using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CH.Test.Mocks;
using CritterHeroes.Web.Domain.Contracts.Email;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.StateManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using TOTD.Mailer.Core;
using TOTD.Utility.UnitTestHelpers;

namespace CH.Test.EmailTests
{
    [TestClass]
    public class EmailBuilderBaseTest
    {
        private string urlLogo = "http://127.0.0.1:10000/devstoreaccount1/fflah/logo%20-%20optimized.svg";

        protected OrganizationContext organizationContext;
        protected Mock<IEmailConfiguration> mockEmailConfiguration;
        protected MockUrlGenerator mockUrlGenerator;
        protected Mock<IStateManager<OrganizationContext>> mockOrganizationStateManager;
        protected Mock<IOrganizationLogoService> mockLogoService;

        [TestInitialize]
        public void InitializeTest()
        {
            organizationContext = new OrganizationContext()
            {
                FullName = "FullName",
                ShortName = "ShortName"
            };

            mockEmailConfiguration = new Mock<IEmailConfiguration>();

            mockUrlGenerator = new MockUrlGenerator();

            mockOrganizationStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrganizationStateManager.Setup(x => x.GetContext()).Returns(organizationContext);

            mockLogoService = new Mock<IOrganizationLogoService>();
            mockLogoService.Setup(x => x.GetLogoUrl()).Returns(urlLogo);
        }

        public void VerifyEmailMessage(EmailCommandBase emailCommand, EmailMessage emailMessage, string emailTitle)
        {
            emailCommand.OrganizationFullName.Should().Be(organizationContext.FullName);
            emailCommand.OrganizationShortName.Should().Be(organizationContext.ShortName);
            emailCommand.UrlLogo.Should().Be(urlLogo);
            emailCommand.UrlHome.Should().NotBeNullOrEmpty();

            emailMessage.Subject.Should().NotBeNullOrEmpty();
            emailMessage.To.Should().Equal(emailCommand.EmailTo);
            emailMessage.From.Should().Be(emailCommand.EmailFrom);

            string folder = Path.Combine(UnitTestHelper.GetSolutionRoot(), "TestResults");
            File.WriteAllText(Path.Combine(folder, $"{emailTitle}.html"), emailMessage.HtmlBody);
            File.WriteAllText(Path.Combine(folder, $"{emailTitle}.txt"), emailMessage.TextBody);
            File.WriteAllText(Path.Combine(folder, $"{emailTitle}.json"), JsonConvert.SerializeObject(emailMessage, Formatting.Indented));
        }
    }
}
