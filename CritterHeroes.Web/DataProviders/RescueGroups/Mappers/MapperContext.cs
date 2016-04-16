using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Events;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Mappers
{
    public class MapperContext<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        public MapperContext(TSource source, TTarget target, IAppEventPublisher publisher)
        {
            this.Source = source;
            this.Target = target;
            this.Publisher = publisher;
        }

        public TSource Source
        {
            get;
        }

        public TTarget Target
        {
            get;
            set;
        }

        public IAppEventPublisher Publisher
        {
            get;
        }
    }
}
