using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Contexts;
using Moq;
using TOTD.EntityFramework;

namespace CH.Test.EntityTests
{
    public class TestSqlStorageContext<T> : ISqlStorageContext<T> where T : class
    {
        private SqlStorageContext<T> _storageContext;

        public TestSqlStorageContext()
        {
            this.MockLogger = new Mock<IHistoryLogger>();
            this._storageContext = new SqlStorageContext<T>(this.MockLogger.Object);
        }

        public IQueryable<T> Entities
        {
            get
            {
                return _storageContext.Entities;
            }
        }

        public Mock<IHistoryLogger> MockLogger
        {
            get;
            private set;
        }

        public void FillWithTestData<EntityType>(EntityType entity, params string[] ignoreProperties)
        {
            _storageContext.FillWithTestData(entity, ignoreProperties);
        }

        public void Add(T entity)
        {
            _storageContext.Add(entity);
        }

        public void Delete(T entity)
        {
            _storageContext.Delete(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _storageContext.GetAll();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _storageContext.GetAllAsync();
        }

        public int SaveChanges()
        {
            return _storageContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _storageContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _storageContext.Dispose();
        }
    }
}
