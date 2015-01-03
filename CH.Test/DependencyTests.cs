using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Account.Queries;
using CritterHeroes.Web.Common.Services.Queries;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using SimpleInjector.Diagnostics;

namespace CH.Test
{
    [TestClass]
    public class DependencyTests
    {
        [TestMethod]
        public void DependencyContainerIsValid()
        {
            Container container = DIConfig.ConfigureDependencyContainer();
            container.Verify();

            IEnumerable<DiagnosticResult> results = Analyzer.Analyze(container);
            results.Any().Should().BeFalse(string.Join(Environment.NewLine, results.Select(x => x.Description)));
        }

        [TestMethod]
        public void DependencyContainerCanResolveQueryHandlers()
        {
            Container container = DIConfig.ConfigureDependencyContainer();
            container.GetRegistration(typeof(IQueryHandler<LoginQuery, LoginModel>)).Should().NotBeNull();
            container.GetRegistration(typeof(IAsyncQueryHandler<UsernameQuery, CheckUsernameResult>)).Should().NotBeNull();
        }

        [TestMethod]
        public void DependencyContainerCanResolveCommandHandlers()
        {
            Container container = DIConfig.ConfigureDependencyContainer();
            container.GetRegistration(typeof(IAsyncCommandHandler<LoginModel>)).Should().NotBeNull();
            container.GetRegistration(typeof(ICommandHandler<LogoutModel>)).Should().NotBeNull();
        }

        [TestMethod]
        public void DependencyContainerCanResolveMasterListDataContexts()
        {
            Container container = DIConfig.ConfigureDependencyContainer();
            container.GetRegistration(typeof(IMasterStorageContext<Breed>)).Should().NotBeNull();
        }

        [TestMethod]
        public void DependencyContainerCanResolveDashboardHandlers()
        {
            Container container = DIConfig.ConfigureDependencyContainer();
            container.GetRegistration(typeof(IDashboardStatusQueryHandler<Breed>)).Should().NotBeNull();
            container.GetRegistration(typeof(IDashboardStatusCommandHandler<AnimalStatus>)).Should().NotBeNull();

            InstanceProducer producer = container.GetRegistration(typeof(IDashboardStatusCommandHandler<Breed>));
            producer.Should().NotBeNull();
            producer.Registration.ImplementationType.Should().Be(typeof(CritterHeroes.Web.Common.Services.CommandHandlers.Dashboard.BreedDashboardStatusCommandHandler));
        }
    }
}
