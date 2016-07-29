using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Models;
using CritterHeroes.Web.Features.Shared.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Features.Admin.Contacts.Queries
{
    public class PersonEditQuery : IAsyncQuery<PersonEditModel>
    {
        public int PersonID
        {
            get;
            set;
        }
    }

    public class PersonEditQueryHandler : IAsyncQueryHandler<PersonEditQuery, PersonEditModel>
    {
        private ISqlQueryStorageContext<Person> _storagePeople;
        private ISqlQueryStorageContext<State> _storageStates;

        public PersonEditQueryHandler(ISqlQueryStorageContext<Person> _storagePeople, ISqlQueryStorageContext<State> storageStates)
        {
            this._storagePeople = _storagePeople;
            this._storageStates = storageStates;
        }

        public async Task<PersonEditModel> ExecuteAsync(PersonEditQuery query)
        {
            Person person = await _storagePeople.Entities.FindByIDAsync(query.PersonID);
            if (person == null)
            {
                throw new InvalidOperationException($"Person ID {query.PersonID} not found");
            }

            PersonEditModel model = new PersonEditModel()
            {
                PersonID = person.ID,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                IsEmailConfirmed = person.IsEmailConfirmed,
                Address = person.Address,
                City = person.City,
                State = person.State,
                Zip = person.Zip
            };

            model.StateOptions = await _storageStates.Entities
                .OrderBy(x => x.Name)
                .SelectToListAsync(x => new StateOptionModel()
                {
                    Value = x.Abbreviation,
                    Text = x.Name,
                    IsSelected = (x.Abbreviation == model.State)
                });

            return model;
        }
    }
}
