using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;
using EntityFramework.Testing.Moq;
using Moq;

namespace CH.Test.Mocks
{
    public class MockSqlCommandStorageContext<T> : Mock<ISqlCommandStorageContext<T>> where T : class
    {
        private MockDbSet<T> _dbset;

        public MockSqlCommandStorageContext()
        {
            _dbset = new MockDbSet<T>().SetupLinq();

            this.Setup(x => x.Entities).Returns(_dbset.Object);

            this.Setup(x => x.Add(It.IsAny<T>())).Callback((T entity) =>
            {
                OnAdd?.Invoke(entity);
            });
        }

        public MockSqlCommandStorageContext(T entity)
            : this()
        {
            AddEntity(entity);
        }

        public MockSqlCommandStorageContext(params T[] entities)
            : this()
        {
            AddEntities(entities);
        }

        public MockSqlCommandStorageContext(IEnumerable<T> entities)
            : this()
        {
            AddEntities(entities);
        }

        public Action<T> OnAdd
        {
            get;
            set;
        }

        public void AddEntity(T entity)
        {
            AddEntities(new[] { entity });
        }

        public void AddEntities(IEnumerable<T> entities)
        {
            _dbset.SetupSeedData(entities);
        }
    }
}
