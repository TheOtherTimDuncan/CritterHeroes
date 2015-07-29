using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using EntityFramework.Testing;
using Moq;

namespace CH.Test.Mocks
{
    public class MockSqlStorageContext<T> : Mock<ISqlStorageContext<T>> where T : class
    {
        public MockSqlStorageContext()
        {
            this.Setup(x => x.Add(It.IsAny<T>())).Callback((T entity) =>
            {
                if (OnAdd != null)
                {
                    OnAdd(entity);
                }
            });
        }

        public MockSqlStorageContext(T entity)
            : this()
        {
            AddEntity(entity);
        }

        public MockSqlStorageContext(params T[] entities)
            : this()
        {
            AddEntities(entities);
        }

        public MockSqlStorageContext(IEnumerable<T> entities)
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

            this.Setup(x => x.Get(It.IsAny<Expression<Func<T, bool>>>())).Returns(entity);
            this.Setup(x => x.GetAsync(It.IsAny<Expression<Func<T, bool>>>())).Returns(Task.FromResult(entity));
        }

        public void AddEntities(IEnumerable<T> entities)
        {
            this.Setup(x => x.Entities).Returns(new TestDbAsyncEnumerable<T>(entities));
            this.Setup(x => x.GetAll()).Returns(entities);
            this.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(entities));
        }
    }
}
