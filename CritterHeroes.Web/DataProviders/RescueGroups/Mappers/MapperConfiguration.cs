using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Mappers
{
    public class MapperConfiguration<TSource, TTarget, TContext>
        where TSource : class
        where TTarget : class
        where TContext : MapperContext<TSource, TTarget>
    {
        private Dictionary<string, FieldMapper> _mappers;

        public MapperConfiguration()
        {
            this._mappers = new Dictionary<string, FieldMapper>();
        }

        public MapperConfiguration<TSource, TTarget, TContext> ForField(string fieldName, Action<TContext> fromAction = null, Action<TContext> toAction = null)
        {
            _mappers.Add(fieldName, new FieldMapper(fromAction, toAction));
            return this;
        }

        public void MapFrom(TContext context, IEnumerable<string> fieldNames = null)
        {
            foreach (var keyValue in _mappers.Where(x => (fieldNames == null || fieldNames.Contains(x.Key)) && x.Value.FromAction != null))
            {
                keyValue.Value.FromAction(context);
            }
        }

        public void MapTo(TContext context, IEnumerable<string> fieldNames = null)
        {
            foreach (var keyValue in _mappers.Where(x => (fieldNames == null || fieldNames.Contains(x.Key)) && x.Value.ToAction != null))
            {
                keyValue.Value.ToAction(context);
            }
        }

        private class FieldMapper
        {
            public FieldMapper(Action<TContext> fromAction, Action<TContext> toAction)
            {
                this.FromAction = fromAction;
                this.ToAction = toAction;
            }

            public Action<TContext> FromAction
            {
                get;
            }

            public Action<TContext> ToAction
            {
                get;
            }
        }
    }
}
