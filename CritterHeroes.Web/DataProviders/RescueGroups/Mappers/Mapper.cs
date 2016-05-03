using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Mappers
{
    public abstract class Mapper<TSource, TTarget, TContext>
        where TSource : class
        where TTarget : class
        where TContext : MapperContext<TSource, TTarget>
    {
        private MapperConfiguration<TSource, TTarget, TContext> _mapperConfiguration;

        public Mapper()
        {
            this._mapperConfiguration = new MapperConfiguration<TSource, TTarget, TContext>();
            CreateConfiguration(this._mapperConfiguration);
        }

        protected abstract void CreateConfiguration(MapperConfiguration<TSource, TTarget, TContext> configuration);

        public void MapTargetToSource(TContext context, IEnumerable<string> fieldNames = null)
        {
            _mapperConfiguration.MapFrom(context, fieldNames);
        }

        public void MapSourceToTarget(TContext context, IEnumerable<string> fieldNames = null)
        {
            _mapperConfiguration.MapTo(context, fieldNames);
        }
    }
}
