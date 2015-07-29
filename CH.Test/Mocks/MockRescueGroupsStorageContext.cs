using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using Moq;

namespace CH.Test.Mocks
{
    public class MockRescueGroupsStorageContext<T> : Mock<IRescueGroupsStorageContext<T>> where T : class
    {
        public MockRescueGroupsStorageContext(T entity, string entityID)
        {
            AddEntity(entity, entityID);
        }

        public MockRescueGroupsStorageContext(params T[] entities)
        {
            AddEntities(entities);
        }

        public MockRescueGroupsStorageContext(IEnumerable<T> entities)
            : this()
        {
            AddEntities(entities);
        }

        public void AddEntity(T entity, string entityID)
        {
            AddEntities(new[] { entity });

            this.Setup(x => x.GetAsync(entityID)).Returns(Task.FromResult(entity));
        }

        public void AddEntities(IEnumerable<T> entities)
        {
            this.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(entities));
        }
    }
}
