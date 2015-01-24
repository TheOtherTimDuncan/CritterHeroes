using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web;
using CritterHeroes.Web.Contracts.Dashboard;
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

            IEnumerable<DiagnosticResult> results = Analyzer.Analyze(container).Where(x =>
            {
                if (x.DiagnosticType == DiagnosticType.DisposableTransientComponent && typeof(Controller).IsAssignableFrom(x.ServiceType))
                {
                    // Ignore Transient lifestyle for IDisposable warning for controllers
                    return false;
                }

                return true;
            });
            results.Any().Should().BeFalse(string.Join(Environment.NewLine, results.Select(x => x.Description)));
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
            producer.Registration.ImplementationType.Should().Be(typeof(CritterHeroes.Web.Areas.Admin.DataMaintenance.CommandHandlers.BreedDashboardStatusCommandHandler));
        }
    }
}
