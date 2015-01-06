using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CritterHeroes.Web;
using CritterHeroes.Web.Common.Dispatchers;
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
        public async Task DependencyContainerCanResolveQueryHandlers()
        {
            Container container = DIConfig.ConfigureDependencyContainer(Assembly.GetExecutingAssembly());
            QueryDispatcher dispatcher = new QueryDispatcher(container);

            TestQuery testQuery = new TestQuery()
            {
                TestValue = "testvalue"
            };
            TestResult testResult = dispatcher.Dispatch(testQuery);
            testResult.Should().NotBeNull();
            testResult.Result.Should().Be(testQuery.TestValue);

            TestAsyncQuery testAsyncQuery = new TestAsyncQuery()
            {
                TestValue = "testasyncvalue"
            };
            TestResult testAsyncResult = await dispatcher.DispatchAsync(testAsyncQuery);
            testAsyncResult.Should().NotBeNull();
            testAsyncResult.Result.Should().Be(testAsyncQuery.TestValue);
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

    public class TestResult
    {
        public string Result
        {
            get;
            set;
        }
    }

    public class TestQuery : IQuery<TestResult>
    {
        public string TestValue
        {
            get;
            set;
        }
    }

    public class TestAsyncQuery : IAsyncQuery<TestResult>
    {
        public string TestValue
        {
            get;
            set;
        }
    }

    public class TestQueryHandler : IQueryHandler<TestQuery, TestResult>
    {
        public TestResult Retrieve(TestQuery query)
        {
            return new TestResult()
            {
                Result = query.TestValue
            };
        }
    }

    public class TestAsyncQueryHandler : IAsyncQueryHandler<TestAsyncQuery, TestResult>
    {
        public Task<TestResult> RetrieveAsync(TestAsyncQuery query)
        {
            return Task.FromResult(new TestResult()
            {
                Result = query.TestValue
            });
        }
    }
}
