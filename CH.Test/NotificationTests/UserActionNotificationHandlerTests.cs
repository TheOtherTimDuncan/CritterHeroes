using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.NotificationHandlers;
using CritterHeroes.Web.Common.Notifications;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.CommandTests
{
    [TestClass]
    public class UserActionNotificationHandlerTests
    {
        [TestMethod]
        public async Task LogsUserAction()
        {
            UserActionNotification notification = new UserActionNotification(UserActions.PasswordLoginSuccess, "username");

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();
            mockUserLogger.Setup(x => x.LogActionAsync(notification.Action, notification.Username, (object)null)).Returns(Task.FromResult(0));

            UserActionNotificationHandler handler = new UserActionNotificationHandler(mockUserLogger.Object);
            await handler.ExecuteAsync(notification);

            mockUserLogger.Verify(x => x.LogActionAsync(notification.Action, notification.Username, (object)null), Times.Once);
        }
    }
}
