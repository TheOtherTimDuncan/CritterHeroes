using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Azure.Storage.Logging;
using CH.Domain.Models;
using CH.Domain.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure.StorageEntityTests
{
    [TestClass]
    public class EmailLogStorageEntityTests : BaseStorageEntityTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToAndFromStorage()
        {
            EmailLog emailLog = new EmailLog(DateTime.UtcNow, new EmailMessage()
            {
                From = "from@from.com",
                HtmlBody = "html",
                TextBody = "text",
                Subject = "subject"
            })
            {
                ForUserID = "userid"
            };
            emailLog.Message.To.Add("to@to.com");

            EmailLogStorageEntity sourceEntity = new EmailLogStorageEntity();
            sourceEntity.Entity = emailLog;

            EmailLogStorageEntity targetEntity = new EmailLogStorageEntity();
            targetEntity.TableEntity = sourceEntity.TableEntity;

            targetEntity.Entity.ID.Should().Be(emailLog.ID);
            targetEntity.Entity.ForUserID.Should().Be(emailLog.ForUserID);
            targetEntity.Entity.WhenSentUtc.Should().Be(emailLog.WhenSentUtc);
            targetEntity.Entity.WhenSentUtc.Kind.Should().Be(DateTimeKind.Utc);

            targetEntity.Entity.Message.Should().NotBeNull();
            targetEntity.Entity.Message.From.Should().Be(emailLog.Message.From);
            targetEntity.Entity.Message.HtmlBody.Should().Be(emailLog.Message.HtmlBody);
            targetEntity.Entity.Message.TextBody.Should().Be(emailLog.Message.TextBody);
            targetEntity.Entity.Message.Subject.Should().Be(emailLog.Message.Subject);
            targetEntity.Entity.Message.To.Should().Equal(emailLog.Message.To);
        }
    }
}
