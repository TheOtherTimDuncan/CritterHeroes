using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.People.Commands;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.Misc;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Admin.People.CommandHandlers
{
    public class ImportPeopleCommandHandler : IAsyncCommandHandler<ImportPeopleCommand>
    {
        private IRescueGroupsStorageContext<PersonSource> _sourceStorage;
        private ISqlStorageContext<Person> _personStorage;
        private ISqlStorageContext<Group> _groupStorage;
        private ISqlStorageContext<PhoneType> _phoneTypeStorage;

        public ImportPeopleCommandHandler(IRescueGroupsStorageContext<PersonSource> sourceStorage, ISqlStorageContext<Person> personStorage, ISqlStorageContext<Group> groupStorage, ISqlStorageContext<PhoneType> phoneTypeStorage)
        {
            this._sourceStorage = sourceStorage;
            this._personStorage = personStorage;
            this._groupStorage = groupStorage;
            this._phoneTypeStorage = phoneTypeStorage;
        }

        public async Task<CommandResult> ExecuteAsync(ImportPeopleCommand command)
        {
            IEnumerable<PersonSource> sources = await _sourceStorage.GetAllAsync();

            if (!sources.IsNullOrEmpty())
            {
                IEnumerable<PhoneType> phoneTypes = await _phoneTypeStorage.GetAllAsync();

                foreach (PersonSource source in sources)
                {
                    Person person = await _personStorage.Entities.FindByRescueGroupsIDAsync(source.ID);
                    if (person == null)
                    {
                        person = new Person()
                        {
                            RescueGroupsID = source.ID
                        };
                        _personStorage.Add(person);
                    }

                    person.FirstName = source.FirstName.EmptyToNull();
                    person.LastName = source.LastName.EmptyToNull();
                    person.Email = source.Email.EmptyToNull();
                    person.Address = source.Address.EmptyToNull();
                    person.City = source.City.EmptyToNull();
                    person.State = source.State.EmptyToNull();
                    person.Zip = source.Zip.EmptyToNull();
                    person.IsActive = source.IsActive;

                    ImportPhoneNumber(person, phoneTypes.Single(x => x.Name.SafeEquals("Home")), source.PhoneHome);
                    ImportPhoneNumber(person, phoneTypes.Single(x => x.Name.SafeEquals("Work")), source.PhoneWork, source.PhoneWorkExtension.EmptyToNull());
                    ImportPhoneNumber(person, phoneTypes.Single(x => x.Name.SafeEquals("Cell")), source.PhoneCell);
                    ImportPhoneNumber(person, phoneTypes.Single(x => x.Name.SafeEquals("Fax")), source.PhoneFax);

                    if (!source.GroupNames.IsNullOrEmpty())
                    {
                        foreach (string groupName in source.GroupNames)
                        {
                            if (!person.Groups.Any(x => x.Group.IfNotNull(g => g.Name) == groupName))
                            {
                                Group group = await _groupStorage.Entities.FindByNameAsync(groupName);
                                if (group == null)
                                {
                                    group = new Group(groupName);
                                    _groupStorage.Add(group);
                                    await _groupStorage.SaveChangesAsync();
                                }
                                person.AddGroup(group.ID);
                            }
                        }
                    }
                    else
                    {
                        person.Groups.Clear();
                    }

                    await _personStorage.SaveChangesAsync();
                }
            }

            return CommandResult.Success();
        }

        private void ImportPhoneNumber(Person person, PhoneType phoneType, string phoneNumber)
        {
            ImportPhoneNumber(person, phoneType, phoneNumber, null);
        }

        private void ImportPhoneNumber(Person person, PhoneType phoneType, string phoneNumber, string phoneExtension)
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
    }
}
