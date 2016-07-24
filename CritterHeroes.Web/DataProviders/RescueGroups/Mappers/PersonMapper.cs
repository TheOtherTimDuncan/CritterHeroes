using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.Misc;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Mappers
{
    public class PersonMapperContext : BaseContactMapperContext<PersonSource, Person>
    {
        public PersonMapperContext(PersonSource source, Person target, IAppEventPublisher publisher)
            : base(source, target, publisher)
        {
        }
    }

    public class PersonMapper : BaseContactMapper<PersonSource, Person, PersonMapperContext>
    {
        protected override void CreateConfiguration(MapperConfiguration<PersonSource, Person, PersonMapperContext> configuration)
        {
            base.CreateConfiguration(configuration);

            configuration
                .ForField
                (
                    fieldName: "contactFirstName",
                    fromAction: context => context.Source.FirstName = context.Target.FirstName,
                    toAction: context => context.Target.FirstName = context.Source.FirstName
                )
                .ForField
                (
                    fieldName: "contactLastName",
                    fromAction: context => context.Source.LastName = context.Target.LastName,
                    toAction: context => context.Target.LastName = context.Source.LastName
                )
                .ForField
                (
                    fieldName: "contactActive",
                    fromAction: context => context.Source.IsActive = context.Target.IsActive,
                    toAction: context => context.Target.IsActive = context.Source.IsActive
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
                    fieldName: "contactPhoneHome",
                    fromAction: context => SetPhoneNumber(context.PhoneTypeHome, context.Target, (phone, ext) => context.Source.PhoneHome = phone),
                    toAction: context => ImportPhoneNumber(context.Target, context.PhoneTypeHome, context.Source.PhoneHome)
                )
                .ForField
                (
                    fieldName: "contactPhoneCell",
                    fromAction: context => SetPhoneNumber(context.PhoneTypeCell, context.Target, (phone, ext) => context.Source.PhoneCell = phone),
                    toAction: context => ImportPhoneNumber(context.Target, context.PhoneTypeCell, context.Source.PhoneCell)
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

        private void ImportPhoneNumber(Person person, PhoneType phoneType, string phoneNumber, string phoneExtension = null)
        {
            PersonPhone personPhone = person.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == phoneType.ID);

            if (!phoneNumber.IsNullOrEmpty())
            {
                if (personPhone == null)
                {
                    person.AddPhoneNumber(phoneNumber, phoneExtension, phoneType.ID);
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
                    person.PhoneNumbers.Remove(personPhone);
                }
            }
        }

        private void SetPhoneNumber(PhoneType phoneType, Person person, Action<string, string> action)
        {
            PersonPhone phone = person.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == phoneType.ID);
            if (phone != null)
            {
                action(phone.PhoneNumber, phone.PhoneExtension);
            }
            else
            {
                action(null, null);
            }
        }

        private void ImportGroups(Person person, IEnumerable<string> sourceGroups, IEnumerable<Group> targetGroups)
        {
            IEnumerable<PersonGroup> deleted = person.Groups.Where(x => !sourceGroups.Any(s => s.SafeEquals(x.Group.Name))).ToList();
            deleted.NullSafeForEach(x => person.Groups.Remove(x));

            foreach (string groupName in sourceGroups)
            {
                Group group = targetGroups.Single(x => x.Name == groupName);
                if (!person.Groups.Any(x => x.GroupID == group.ID))
                {
                    person.AddGroup(group.ID);
                }
            }
        }
    }
}
