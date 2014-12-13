﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts.Logging;
using CH.Domain.Models;
using CH.Domain.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CH.Test.Azure
{
    [TestClass]
    public class UserLogTests : BaseTest
    {
        [TestMethod]
        public async Task CanSaveAndRetrieveUserLog()
        {
            string testUsername = "test.user";
            UserActions userAction = UserActions.PasswordLoginSuccess;

            IUserLogger userLogger = Using<IUserLogger>();
            await userLogger.LogActionAsync(userAction, testUsername);

            IEnumerable<UserLog> userLogs = await userLogger.GetUserLogAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));
            UserLog log = userLogs.FirstOrDefault(x => x.Username == testUsername);
            log.Should().NotBeNull();
            log.Action.Should().Be(userAction);
        }

        [TestMethod]
        public async Task CanSaveAndRetrieveUserLogWithAdditionalData()
        {
            string testUsername = "test.user";
            UserActions userAction = UserActions.PasswordLoginSuccess;

            EmailMessage message = new EmailMessage()
            {
                From = "from",
                Subject = "subject",
                HtmlBody = "html",
                TextBody = "text"
            };
            message.To.Add("to");

            string additionalData = JsonConvert.SerializeObject(message);

            IUserLogger userLogger = Using<IUserLogger>();
            await userLogger.LogActionAsync(userAction, testUsername, message);

            IEnumerable<UserLog> userLogs = await userLogger.GetUserLogAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));
            UserLog log = userLogs.FirstOrDefault(x => x.Username == testUsername && x.AdditionalData != null);
            log.Should().NotBeNull();
            log.Action.Should().Be(userAction);
            log.AdditionalData = additionalData;
        }
    }
}
