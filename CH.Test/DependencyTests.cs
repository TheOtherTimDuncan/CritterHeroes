using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Dashboard;
using CH.Domain.Contracts.Queries;
using CH.Domain.Contracts.Storage;
using CH.Domain.Models.Data;
using CH.Domain.Services.Queries;
using CH.Domain.StateManagement;
using CH.Website;
using CH.Website.Dependency;
using CH.Website.Models.Account;
using CH.Website.Services.Queries;
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
            Container container = CH.Website.DIConfig.ConfigureDependencyContainer();
            container.Verify();

            IEnumerable<DiagnosticResult> results = Analyzer.Analyze(container);
            results.Any().Should().BeFalse(string.Join(Environment.NewLine, results.Select(x => x.Description)));
        }

        [TestMethod]
        public void DependencyContainerCanResolveQueryHandlers()
        {
            Container container = CH.Website.DIConfig.ConfigureDependencyContainer();
            container.GetRegistration(typeof(IQueryHandler<LoginQuery, LoginModel>)).Should().NotBeNull();
            container.GetRegistration(typeof(IAsyncQueryHandler<UsernameQuery, CheckUsernameResult>)).Should().NotBeNull();
        }

        [TestMethod]
        public void DependencyContainerCanResolveCommandHandlers()
        {
            Container container = CH.Website.DIConfig.ConfigureDependencyContainer();
            container.GetRegistration(typeof(IAsyncCommandHandler<LoginModel>)).Should().NotBeNull();
            container.GetRegistration(typeof(ICommandHandler<LogoutModel>)).Should().NotBeNull();
        }

        [TestMethod]
        public void DependencyContainerCanResolveMasterListDataContexts()
        {
            Container container = CH.Website.DIConfig.ConfigureDependencyContainer();
            container.GetRegistration(typeof(IMasterStorageContext<Breed>)).Should().NotBeNull();
        }

        [TestMethod]
        public void DependencyContainerCanResolveDashboardHandlers()
        {
            Container container = CH.Website.DIConfig.ConfigureDependencyContainer();
            container.GetRegistration(typeof(IDashboardStatusQueryHandler<Breed>)).Should().NotBeNull();
            container.GetRegistration(typeof(IDashboardStatusCommandHandler<AnimalStatus>)).Should().NotBeNull();

            InstanceProducer producer = container.GetRegistration(typeof(IDashboardStatusCommandHandler<Breed>));
            producer.Should().NotBeNull();
            producer.Registration.ImplementationType.Should().Be(typeof(CH.Domain.Services.CommandHandlers.Dashboard.BreedDashboardStatusCommandHandler));
        }
    }
}
