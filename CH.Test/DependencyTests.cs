using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CritterHeroes.Web;
using CritterHeroes.Web.Common.Dispatchers;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Notifications;
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
        public async Task DependencyContainerCanResolveNotificationHandlers()
        {
            Container container = DIConfig.ConfigureDependencyContainer(Assembly.GetExecutingAssembly());
            NotificationPublisher publisher = new NotificationPublisher(container);

            TestNotification notification = new TestNotification();

            publisher.Publish(notification);
            await publisher.PublishAsync(notification);

            notification.Value.Should().Be(4, "there are 2 synchronous and 2 asynchronous handlers registered for this notification");
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

    public class TestNotification : INotification, IAsyncNotification
    {
        public int Value
        {
            get;
            set;
        }
    }

    public class TestNotificationHandler1 : INotificationHandler<TestNotification>
    {
        void INotificationHandler<TestNotification>.Execute(TestNotification notification)
        {
            notification.Value++;
        }
    }

    public class TestNotificationHandler2 : INotificationHandler<TestNotification>
    {
        void INotificationHandler<TestNotification>.Execute(TestNotification notification)
        {
            notification.Value++;
        }
    }

    public class TestAsyncNotificationHandler1 : IAsyncNotificationHandler<TestNotification>
    {
        public async Task ExecuteAsync(TestNotification notification)
        {
            notification.Value++;
        }
    }

    public class TestAsyncNotificationHandler2 : IAsyncNotificationHandler<TestNotification>
    {
        public async Task ExecuteAsync(TestNotification notification)
        {
            notification.Value++;
        }
    }
}
