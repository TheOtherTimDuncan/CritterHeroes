using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Critters.Commands;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Admin.Critters.CommandHandlers
{
    public class ImportCrittersCommandHandler : IAsyncCommandHandler<ImportCrittersCommand>
    {
        private IRescueGroupsStorageContext<CritterSearchResult> _sourceStorage;
        private ICritterBatchSqlStorageContext _critterStorage;
        private IStateManager<OrganizationContext> _stateManager;

        public ImportCrittersCommandHandler(ICritterBatchSqlStorageContext critterStorage, IStateManager<OrganizationContext> stateManager, IRescueGroupsStorageContext<CritterSearchResult> sourceStorage)
        {
            this._critterStorage = critterStorage;
            this._stateManager = stateManager;
            this._sourceStorage = sourceStorage;
        }

        public async Task<CommandResult> ExecuteAsync(ImportCrittersCommand command)
        {
            IEnumerable<CritterSearchResult> sources = await _sourceStorage.GetAllAsync();

            OrganizationContext orgContext = _stateManager.GetContext();

            foreach (CritterSearchResult source in sources)
            {
                CritterStatus status = await GetCritterStatusAsync(source.StatusID, source.Status);
                Breed breed = await GetBreedAsync(source.PrimaryBreedID, source.PrimaryBreed, source.Species);

                Critter critter = await _critterStorage.Critters.FindByRescueGroupsIDAsync(source.ID);
                if (critter == null)
                {
                    critter = new Critter(source.Name, status, breed, orgContext.OrganizationID, source.ID);
                    _critterStorage.AddCritter(critter);
                }
                else
                {
                    critter.Name = source.Name;

                    if (critter.BreedID != breed.ID)
                    {
                        critter.ChangeBreed(breed);
                    }

                    if (critter.StatusID != status.ID)
                    {
                        critter.ChangeStatus(status);
                    }
                }

                critter.Sex = source.Sex;
                critter.WhenUpdated = DateTimeOffset.UtcNow;
                critter.RescueID = source.RescueID;

                if (!source.FosterContactID.IsNullOrEmpty())
                {
                    Person person =await  GetFosterAsync(source.FosterContactID, source.FosterFirstName, source.FosterLastName, source.FosterEmail);
                    critter.ChangePerson(person);
                }
                else
                {
                    critter.RemovePerson();
                }

                if (!source.LastUpdated.IsNullOrEmpty())
                {
                    critter.RescueGroupsLastUpdated = DateTime.Parse(source.LastUpdated);
                }

                await _critterStorage.SaveChangesAsync();
            }

            return CommandResult.Success();
        }

        private async Task<CritterStatus> GetCritterStatusAsync(string statusID, string status)
        {
            CritterStatus critterStatus = await _critterStorage.CritterStatus.FindByRescueGroupsIDAsync(statusID);
            if (critterStatus == null)
            {
                critterStatus = new CritterStatus(status, status, statusID);
            }
            return critterStatus;
        }

        private async Task<Breed> GetBreedAsync(string breedID, string breedName, string speciesName)
        {
            Breed breed = await _critterStorage.Breeds.FindByRescueGroupsIDAsync(breedID);
            if (breed == null)
            {
                Species species = await _critterStorage.Species.FindByNameAsync(speciesName);
                if (species == null)
                {
                    species = new Species(speciesName, speciesName, speciesName, null, null);
                }

                breed = new Breed(species.ID, breedName, breedID);
            }
            return breed;
        }

        private async Task<Person> GetFosterAsync(string fosterID, string firstName, string lastName, string email)
        {
            Person person = await _critterStorage.People.FindByRescueGroupsIDAsync(fosterID);

            if (person == null)
            {
                person = new Person()
                {
                    RescueGroupsID = fosterID,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email
                };
            }

            return person;
        }
    }
}
