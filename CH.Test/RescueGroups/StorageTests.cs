using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using CritterHeroes.Web.Models.LogEvents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void CanReadErrorResponse()
        {
            MockHttpClient mockHttpClient = new MockHttpClient()
                .SetupListResponse("{\"status\":\"error\",\"messages\":{\"generalMessages\":[{\"messageID\":\"1001\",\"messageCriticality\":\"error\",\"messageText\":\"The action you specified was not found.\"}],\"recordMessages\":[]},\"foundRows\":0,\"data\":[]}");

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            TestSourceStorage storage = new TestSourceStorage(new RescueGroupsConfiguration(), mockHttpClient.Object, mockPublisher.Object);
            storage._isPrivate = false;

            Action action = () =>
            {
                var result = storage.GetAllAsync().Result;
            };

            action.ShouldThrow<RescueGroupsException>().WithMessage("The action you specified was not found.");

            mockPublisher.Verify(x => x.Publish(It.IsAny<RescueGroupsLogEvent>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task CanReadResponseWithNoData()
        {
            MockHttpClient mockHttpClient = new MockHttpClient()
                .SetupListResponse("{\"status\":\"ok\",\"messages\":{\"generalMessages\":[],\"recordMessages\":[]},\"foundRows\":0,\"data\":[]}");

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            TestSourceStorage storage = new TestSourceStorage(new RescueGroupsConfiguration(), mockHttpClient.Object, mockPublisher.Object);
            storage._isPrivate = false;

            IEnumerable<TestSource> result = await storage.GetAllAsync();
            result.Should().BeEmpty();

            mockPublisher.Verify(x => x.Publish(It.IsAny<RescueGroupsLogEvent>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task SendsLoginRequestIfIsPrivate()
        {
            string token = "validtoken";
            string tokenHash = "validtokenhash";

            MockHttpClient mockHttpClient = new MockHttpClient()
                .SetupResponseForAction(ObjectActions.Login, $"{{\"status\":\"ok\", data: {{ \"token\": \"{token}\", \"tokenHash\": \"{tokenHash}\" }} }}")
                .SetupListResponse("{\"status\":\"ok\",\"messages\":{\"generalMessages\":[],\"recordMessages\":[]},\"foundRows\":0,\"data\":[]}")
                .Callback(ObjectActions.List, (string json) =>
                 {
                     json.Should().Contain(token);
                     json.Should().Contain(tokenHash);
                 });

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            TestSourceStorage storage = new TestSourceStorage(new RescueGroupsConfiguration(), mockHttpClient.Object, mockPublisher.Object);
            storage._isPrivate = true;

            IEnumerable<TestSource> result = await storage.GetAllAsync();
            result.Should().BeEmpty();

            mockPublisher.Verify(x => x.Publish(It.IsAny<RescueGroupsLogEvent>()), Times.Exactly(2));
        }

        private class TestSource
        {
        }

        private class TestSourceStorage : RescueGroupsStorage<TestSource>
        {
            public IEnumerable<SearchField> _fields;
            public bool _isPrivate;
            public string _objectType;
            public string _keyField;
            public string _sortField;

            public TestSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
                : base(configuration, client, publisher)
            {
                this._isPrivate = false;
                this._fields = null;
                this._objectType = null;
                this._keyField = null;
                this._sortField = null;
            }

            public override IEnumerable<SearchField> Fields
            {
                get;
            }

            public override bool IsPrivate
            {
                get
                {
                    return _isPrivate;
                }
            }

            public override string ObjectType
            {
                get
                {
                    return _objectType;
                }
            }

            protected override string KeyField
            {
                get
                {
                    return _keyField;
                }
            }

            protected override string SortField
            {
                get
                {
                    return _sortField;
                }
            }

            public override IEnumerable<TestSource> FromStorage(IEnumerable<JProperty> tokens)
            {
                return tokens.Select(x => x.Value.ToObject<TestSource>());
            }
        }
    }
}
