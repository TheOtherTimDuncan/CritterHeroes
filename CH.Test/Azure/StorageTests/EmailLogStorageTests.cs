using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure.StorageTests
{
    [TestClass]
    public class EmailLogStorageTests : BaseAzureTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToAndFromStorage()
        {
            EmailMessage message = new EmailMessage();
            message.To.Add("to@to.com");

            EmailLog emailLog = new EmailLog(DateTimeOffset.UtcNow, message);
            emailLog.EmailTo.Should().Be(message.To.Single());

            AzureEmailLogger source = new AzureEmailLogger(new AzureConfiguration(), null);
            AzureEmailLogger target = new AzureEmailLogger(new AzureConfiguration(), null);
            EmailLog result = target.FromStorage(source.ToStorage(emailLog));

            result.ID.Should().Be(emailLog.ID);
            result.WhenSentUtc.Should().Be(emailLog.WhenSentUtc);
        }
    }
}
