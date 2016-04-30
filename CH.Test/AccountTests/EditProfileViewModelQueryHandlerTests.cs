﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Common.Queries;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Features.Account.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AccountTests
{
    [TestClass]
    public class EditProfileViewModelQueryHandlerTests
    {
        [TestMethod]
        public async Task EditProfileQueryHandlerReturnsViewModel()
        {
            AppUser user = new AppUser("email@email.com");
            user.Person.FirstName = "First";
            user.Person.LastName = "Last";

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.Username).Returns(user.UserName);

            MockSqlStorageContext<AppUser> mockUserStorageContext = new MockSqlStorageContext<AppUser>(user);

            EditProfileQueryHandler handler = new EditProfileQueryHandler(mockHttpUser.Object, mockUserStorageContext.Object);
            EditProfileModel model = await handler.ExecuteAsync(new UserIDQuery());

            model.Should().NotBeNull();
            model.FirstName.Should().Be(user.Person.FirstName);
            model.LastName.Should().Be(user.Person.LastName);
            model.Email.Should().Be(user.Email);
        }
    }
}
