using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Domain.Contracts.Events;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Models.LogEvents;
using CritterHeroes.Web.Shared;
using Moq;
using Serilog;
using TOTD.EntityFramework;

namespace CH.Test.EntityTests
{
    public class TestSqlStorageContext<T> : ISqlCommandStorageContext<T> where T : class
    {
        private SqlCommandStorageContext<T> _storageContext;
        private ILogger _logger;

        public TestSqlStorageContext()
        {
            LogMessages = new List<string>();

            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.List(LogMessages)
                .CreateLogger();

            this.MockPublisher = new Mock<IAppEventPublisher>();
            this.MockPublisher.Setup(x => x.Publish(It.IsAny<HistoryLogEvent>())).Callback((HistoryLogEvent logEvent) =>
            {
                HistoryLogEvent.HistoryContext context = (HistoryLogEvent.HistoryContext)logEvent.Context;

                HistoryBefore = context.Before;
                HistoryAfter = context.After;

                _logger
                    .ForContext("Context", logEvent.Context, destructureObjects: true)
                    .Write(logEvent.Level, logEvent.MessageTemplate, logEvent.MessageValues);
            });

            this._storageContext = new SqlCommandStorageContext<T>(this.MockPublisher.Object);
        }

        public IQueryable<T> Entities
        {
            get
            {
                return _storageContext.Entities;
            }
        }

        public Mock<IAppEventPublisher> MockPublisher
        {
            get;
            private set;
        }

        public string HistoryBefore
        {
            get;
            set;
        }

        public string HistoryAfter
        {
            get;
            set;
        }

        public List<string> LogMessages
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
