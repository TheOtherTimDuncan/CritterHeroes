using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using EntityFramework.Testing;
using EntityFramework.Testing.Moq;
using Moq;

namespace CH.Test.Mocks
{
    public class MockSqlStorageContext<T> : Mock<ISqlStorageContext<T>> where T : class
    {
        private MockDbSet<T> _dbset;

        public MockSqlStorageContext()
        {
            _dbset = new MockDbSet<T>()
                .SetupAddAndRemove()
                .SetupLinq();

            _dbset.Setup(x => x.AsNoTracking()).Returns(_dbset.Object);

            this.Setup(x => x.Entities).Returns(_dbset.Object);
            this.Setup(x => x.GetAll()).Returns(() => _dbset.Object.ToList());
            this.Setup(x => x.GetAllAsync()).Returns(async () => await _dbset.Object.ToListAsync());
            this.Setup(x => x.Delete(It.IsAny<T>())).Callback((T entity) => _dbset.Object.Remove(entity));

            this.Setup(x => x.Add(It.IsAny<T>())).Callback((T entity) =>
            {
                _dbset.Object.Add(entity);

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
        }

        public void AddEntities(IEnumerable<T> entities)
        {
            _dbset.SetupSeedData(entities);
        }
    }
}
