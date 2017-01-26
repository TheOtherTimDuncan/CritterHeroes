using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.Storage;
using EntityFramework.Testing.Moq;
using Moq;

namespace CH.Test.Mocks
{
    public class MockSqlQueryStorageContext<T> : Mock<ISqlQueryStorageContext<T>> where T : class
    {
        private MockDbSet<T> _dbset;

        public MockSqlQueryStorageContext()
        {
            _dbset = new MockDbSet<T>().SetupLinq();

            _dbset.Setup(x => x.AsNoTracking()).Returns(_dbset.Object);

            this.Setup(x => x.Entities).Returns(_dbset.Object);
            this.Setup(x => x.GetAll()).Returns(() => _dbset.Object.ToList());
            this.Setup(x => x.GetAllAsync()).Returns(async () => await _dbset.Object.ToListAsync());
        }

        public MockSqlQueryStorageContext(T entity)
            : this()
        {
            AddEntity(entity);
        }

        public MockSqlQueryStorageContext(params T[] entities)
            : this()
        {
            AddEntities(entities);
        }

        public MockSqlQueryStorageContext(IEnumerable<T> entities)
            : this()
        {
            AddEntities(entities);
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
