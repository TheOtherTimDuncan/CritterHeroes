using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Mappers
{
    public class BusinessMapperContext : BaseContactMapperContext<BusinessSource, Business>
    {
        public BusinessMapperContext(BusinessSource source, Business target, IAppEventPublisher publisher)
            : base(source, target, publisher)
        {
        }
    }

    public class BusinessMapper : BaseContactMapper<BusinessSource, Business, BusinessMapperContext>
    {
        protected override void CreateConfiguration(MapperConfiguration<BusinessSource, Business, BusinessMapperContext> configuration)
        {
            base.CreateConfiguration(configuration);

            configuration
                .ForField
                (
                    fieldName: "contactCompany",
                    fromAction: context => context.Source.Company = context.Target.Name,
                    toAction: context => context.Target.Name = context.Source.Company
                )
                .ForField
                (
                    fieldName: "contactEmail",
                    fromAction: context => context.Source.Email = context.Target.Email,
                    toAction: context => context.Target.Email = context.Source.Email
                )
                .ForField
                (
                    fieldName: "contactPhoneWork",
                    fromAction: context => SetPhoneNumber(context.PhoneTypeWork, context.Target, (phone, ext) =>
                    {
                        context.Source.PhoneWork = phone;
                        context.Source.PhoneWorkExtension = ext;
                    }),
                    toAction: context => ImportPhoneNumber(context.Target, context.PhoneTypeWork, context.Source.PhoneWork, context.Source.PhoneWorkExtension)
                )
                .ForField
                (
                    fieldName: "contactFax",
                    fromAction: context => SetPhoneNumber(context.PhoneTypeFax, context.Target, (phone, ext) => context.Source.PhoneFax = phone),
                    toAction: context => ImportPhoneNumber(context.Target, context.PhoneTypeFax, context.Source.PhoneFax)
                )
                .ForField
                (
                    fieldName: "contactGroups",
                    toAction: context => ImportGroups(context.Target, context.Source.GroupNames, context.Groups)
                );
        }

        private void ImportPhoneNumber(Business business, PhoneType phoneType, string phoneNumber, string phoneExtension = null)
        {
            BusinessPhone personPhone = business.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == phoneType.ID);

            if (!phoneNumber.IsNullOrEmpty())
            {
                if (personPhone == null)
                {
                    business.AddPhoneNumber(phoneNumber, phoneExtension, phoneType.ID);
                }
                else
                {
                    personPhone.PhoneNumber = phoneNumber;
                    personPhone.PhoneExtension = phoneExtension;
                }
            }
            else
            {
                if (personPhone != null)
                {
                    business.PhoneNumbers.Remove(personPhone);
                }
            }
        }

        private void SetPhoneNumber(PhoneType phoneType, Business business, Action<string, string> action)
        {
            BusinessPhone phone = business.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == phoneType.ID);
            if (phone != null)
            {
                action(phone.PhoneNumber, phone.PhoneExtension);
            }
            else
            {
                action(null, null);
            }
        }

        private void ImportGroups(Business business, IEnumerable<string> sourceGroups, IEnumerable<Group> targetGroups)
        {
            IEnumerable<BusinessGroup> deleted = business.Groups.Where(x => !sourceGroups.Any(s => s.SafeEquals(x.Group.Name))).ToList();
            deleted.NullSafeForEach(x => business.Groups.Remove(x));

            foreach (string groupName in sourceGroups)
            {
                Group group = targetGroups.Single(x => x.Name == groupName);
                if (!business.Groups.Any(x => x.GroupID == group.ID))
                {
                    business.AddGroup(group.ID);
                }
            }
        }
    }
}
