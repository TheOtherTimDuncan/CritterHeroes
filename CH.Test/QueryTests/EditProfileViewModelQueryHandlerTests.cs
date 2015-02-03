using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Account.QueryHandlers;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Queries;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.QueryTests
{
    [TestClass]
    public class EditProfileViewModelQueryHandlerTests
    {
        [TestMethod]
        public async Task EditProfileQueryHandlerReturnsViewModel()
        {
            UserIDQuery query = new UserIDQuery();

            string uriReferrer = "http://google.com";

            IdentityUser user = new IdentityUser(query.UserID, "email@email.com")
            {
                FirstName = "First",
                LastName = "Last"
            };

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.UserID).Returns(user.Id);

            Mock<IHeaderDictionary> mockHeaderDictionary = new Mock<IHeaderDictionary>();
            string[] headerValue = new string[] { uriReferrer };
            mockHeaderDictionary.Setup(x => x.TryGetValue(It.IsAny<string>(), out headerValue)).Returns(true);

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.Headers).Returns(mockHeaderDictionary.Object);

            Mock<IApplicationUserStore> mockUserStore = new Mock<IApplicationUserStore>();
            mockUserStore.Setup(x => x.FindByIdAsync(query.UserID)).Returns(Task.FromResult(user));

            EditProfileViewModelQueryHandler handler = new EditProfileViewModelQueryHandler(mockOwinContext.Object, mockHttpUser.Object, mockUserStore.Object);
            EditProfileModel model = await handler.RetrieveAsync(query);

            model.Should().NotBeNull();
            model.FirstName.Should().Be(user.FirstName);
            model.LastName.Should().Be(user.LastName);
            model.Email.Should().Be(user.Email);
            model.ReturnUrl.Should().Be(uriReferrer);

            mockUserStore.Verify(x => x.FindByIdAsync(query.UserID), Times.Once);
        }
    }
}
