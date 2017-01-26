using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Domain.Contracts.Events;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Mappers
{
    public class BaseContactMapperContext<TSource, TTarget> : MapperContext<TSource, TTarget>
        where TSource : BaseContactSource
        where TTarget : BaseContact
    {
        public BaseContactMapperContext(TSource source, TTarget target, IAppEventPublisher publisher)
            : base(source, target, publisher)
        {
        }

        public IEnumerable<Group> Groups
        {
            get;
            set;
        }

        public IEnumerable<PhoneType> PhoneTypes
        {
            get;
            set;
        }

        public PhoneType PhoneTypeWork
        {
            get
            {
                return PhoneTypes.Single(x => x.Name.SafeEquals(PhoneTypeNames.Work));
            }
        }

        public PhoneType PhoneTypeHome
        {
            get
            {
                return PhoneTypes.Single(x => x.Name.SafeEquals(PhoneTypeNames.Home));
            }
        }

        public PhoneType PhoneTypeCell
        {
            get
            {
                return PhoneTypes.Single(x => x.Name.SafeEquals(PhoneTypeNames.Cell));
            }
        }

        public PhoneType PhoneTypeFax
        {
            get
            {
                return PhoneTypes.Single(x => x.Name.SafeEquals(PhoneTypeNames.Fax));
            }
        }
    }

    public  class BaseContactMapper<TSource, TTarget, TContext> : Mapper<TSource, TTarget, TContext>
        where TSource : BaseContactSource
        where TTarget : BaseContact
        where TContext : BaseContactMapperContext<TSource, TTarget>
    {
        protected override void CreateConfiguration(MapperConfiguration<TSource, TTarget, TContext> configuration)
        {
            configuration
                .ForField
                (
                    fieldName: "contactID",
                    fromAction: context => context.Source.ID = context.Target.RescueGroupsID.Value,
                    toAction: context => context.Target.RescueGroupsID = context.Source.ID
                )
                .ForField
                (
                    fieldName: "contactAddress",
                    fromAction: context => context.Source.Address = context.Target.Address,
                    toAction: context => context.Target.Address = context.Source.Address
                )
                .ForField
                (
                    fieldName: "contactCity",
                    fromAction: context => context.Source.City = context.Target.City,
                    toAction: context => context.Target.City = context.Source.City
                )
                .ForField
                (
                    fieldName: "contactState",
                    fromAction: context => context.Source.State = context.Target.State,
                    toAction: context => context.Target.State = context.Source.State
                )
                .ForField
                (
                    fieldName: "contactPostalcode",
                    fromAction: context => context.Source.Zip = context.Target.Zip,
                    toAction: context => context.Target.Zip = context.Source.Zip
                );
        }
    }
}
