using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.StringHelpers;

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

        /// <summary>
        /// All Rescue Groups dates are in Eastern
        /// </summary>
        protected DateTime? DateTimeOffsetToDate(DateTimeOffset? source)
        {
            if (source == null)
            {
                return null;
            }

            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(source.Value, "Eastern Standard Time").DateTime;
        }

        /// <summary>
        /// All Rescue Groups dates are in Eastern
        /// </summary>
        protected DateTimeOffset? DateTimeToDateTimeOffset(DateTime? source)
        {
            if (source == null)
            {
                return null;
            }

            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            TimeSpan offset = timeZone.BaseUtcOffset;

            if (timeZone.IsDaylightSavingTime(source.Value))
            {
                offset = offset.Add(TimeSpan.FromHours(1));
            }

            return new DateTimeOffset(source.Value, offset).ToUniversalTime();
        }
    }
}
