using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.StateManagement;
using CH.Website.Models.Account;
using CH.Website.Services.CommandHandlers;
using FluentAssertions;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.CommandTests
{
    [TestClass]
    public class LogoutCommandHandlerTests
    {
        [TestMethod]
        public void LogsOutUserAndClearsUserState()
        {
            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(x => x.SignOut());

            Mock<IStateManager<UserContext>> mockStateManager = new Mock<IStateManager<UserContext>>();
            mockStateManager.Setup(x => x.ClearContext());

            LogoutCommandHandler handler = new LogoutCommandHandler(mockAuthenticationManager.Object, mockStateManager.Object);
            handler.Execute(new LogoutModel());

            mockAuthenticationManager.Verify(x => x.SignOut(), Times.Once);
            mockStateManager.Verify(x => x.ClearContext(), Times.Once);
        }
    }
}
