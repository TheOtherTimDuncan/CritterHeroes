using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Commands;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.Misc;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Features.Admin.Contacts.CommandHandlers
{
    public class ImportBusinessCommandHandler : IAsyncCommandHandler<ImportBusinessCommand>
    {
        private IRescueGroupsStorageContext<BusinessSource> _sourceStorage;
        private ISqlStorageContext<Business> _businessStorage;
        private ISqlStorageContext<Group> _groupStorage;
        private ISqlStorageContext<PhoneType> _phoneTypeStorage;

        public ImportBusinessCommandHandler(IRescueGroupsStorageContext<BusinessSource> sourceStorage, ISqlStorageContext<Business> businessStorage, ISqlStorageContext<Group> groupStorage, ISqlStorageContext<PhoneType> phoneTypeStorage)
        {
            this._sourceStorage = sourceStorage;
            this._businessStorage = businessStorage;
            this._groupStorage = groupStorage;
            this._phoneTypeStorage = phoneTypeStorage;
        }

        public async Task<CommandResult> ExecuteAsync(ImportBusinessCommand command)
        {
            IEnumerable<BusinessSource> sources = await _sourceStorage.GetAllAsync();

            if (!sources.IsNullOrEmpty())
            {
                IEnumerable<PhoneType> phoneTypes = await _phoneTypeStorage.GetAllAsync();
                PhoneType phoneTypeFax = phoneTypes.Single(x => x.Name.SafeEquals("Fax"));

                foreach (BusinessSource source in sources)
                {
                    Business business = await _businessStorage.Entities.FindByRescueGroupsIDAsync(source.ID);
                    if (business == null)
                    {
                        business = new Business()
                        {
                            RescueGroupsID = source.ID
                        };
                        _businessStorage.Add(business);
                    }

                    business.Name = source.Name.EmptyToNull();
                    business.Email = source.Email.EmptyToNull();
                    business.Address = source.Address.EmptyToNull();
                    business.City = source.City.EmptyToNull();
                    business.State = source.State.EmptyToNull();
                    business.Zip = source.Zip.EmptyToNull();

                    ImportPhoneNumber(business, null, source.PhoneWork, source.PhoneWorkExtension.EmptyToNull());
                    ImportPhoneNumber(business, phoneTypeFax, source.PhoneFax);

                    if (!source.GroupNames.IsNullOrEmpty())
                    {
                        foreach (string groupName in source.GroupNames)
                        {
                            if (!business.Groups.Any(x => x.Group.IfNotNull(g => g.Name) == groupName))
                            {
                                Group group = await _groupStorage.Entities.FindByNameAsync(groupName);
                                if (group == null)
                                {
                                    group = new Group(groupName)
                                    {
                                        IsBusiness = true
                                    };
                                    _groupStorage.Add(group);
                                    await _groupStorage.SaveChangesAsync();
                                }
                                business.AddGroup(group.ID);
                            }
                        }
                    }
                    else
                    {
                        business.Groups.Clear();
                    }

                    await _businessStorage.SaveChangesAsync();
                }
            }

            return CommandResult.Success();
        }

        private void ImportPhoneNumber(Business business, PhoneType phoneType, string phoneNumber)
        {
            ImportPhoneNumber(business, phoneType, phoneNumber, null);
        }

        private void ImportPhoneNumber(Business business, PhoneType phoneType, string phoneNumber, string phoneExtension)
        {
            BusinessPhone businessPhone;
            if (phoneType != null)
            {
                businessPhone = business.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == phoneType.ID);
            }
            else
            {
                businessPhone = business.PhoneNumbers.SingleOrDefault(x => x.PhoneTypeID == null);
            }

            if (!phoneNumber.IsNullOrEmpty())
            {
                if (businessPhone == null)
                {
                    if (phoneType != null)
                    {
                        business.AddPhoneNumber(phoneNumber, phoneExtension, phoneType.ID);
                    }
                    else
                    {
                        business.AddPhoneNumber(phoneNumber, phoneExtension, null);
                    }
                }
                else
                {
                    businessPhone.PhoneNumber = phoneNumber;
                    businessPhone.PhoneExtension = phoneExtension;
                }
            }
            else
            {
                if (businessPhone != null)
                {
                    business.PhoneNumbers.Remove(businessPhone);
                }
            }
        }
    }
}
