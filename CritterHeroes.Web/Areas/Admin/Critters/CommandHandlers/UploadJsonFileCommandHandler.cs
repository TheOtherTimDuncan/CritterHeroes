using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Critters.Commands;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json;

namespace CritterHeroes.Web.Areas.Admin.Critters.CommandHandlers
{
    public class UploadJsonFileCommandHandler : IAsyncCommandHandler<UploadJsonFileCommand>
    {
        private ICritterBatchSqlStorageContext _critterStorage;

        public UploadJsonFileCommandHandler(ICritterBatchSqlStorageContext critterStorage)
        {
            this._critterStorage = critterStorage;
        }

        public async Task<CommandResult> ExecuteAsync(UploadJsonFileCommand command)
        {
            using (StreamReader reader = new StreamReader(command.File.InputStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    CritterSource critterSource = JsonConvert.DeserializeObject<CritterSource>(line);

                    CritterStatus status = await GetCritterStatus(critterSource.Status);
                    Breed breed = await GetBreed(critterSource.PrimaryBreed, critterSource.Species);

                    Critter critter = await _critterStorage.Critters.FindByRescueGroupsIDAsync(critterSource.AnimalID);
                    if (critter == null)
                    {
                        critter = new Critter(status, critterSource.Name, breed, critterSource.AnimalID);
                        _critterStorage.AddCritter(critter);
                    }
                    else
                    {
                        critter.Name = critterSource.Name;

                        if (critter.BreedID != breed.ID)
                        {
                            critter.ChangeBreed(breed);
                        }

                        if (critter.StatusID != status.ID)
                        {
                            critter.ChangeStatus(status);
                        }
                    }

                    critter.Sex = critterSource.Sex;
                    critter.WhenUpdated = DateTimeOffset.UtcNow;

                    await _critterStorage.SaveChangesAsync();
                }
            }

            return CommandResult.Success();
        }

        private async Task<CritterStatus> GetCritterStatus(string status)
        {
            CritterStatus critterStatus = await _critterStorage.CritterStatus.FindByNameAsync(status);
            if (critterStatus == null)
            {
                critterStatus = new CritterStatus(status, status);
            }
            return critterStatus;
        }

        private async Task<Breed> GetBreed(string breedName, string speciesName)
        {
            Breed breed = await _critterStorage.Breeds.FindByNameAsync(breedName);
            if (breed == null)
            {
                Species species = await _critterStorage.Species.FindByNameAsync(speciesName);
                if (species == null)
                {
                    species = new Species(speciesName, speciesName, speciesName, null, null);
                }

                breed = new Breed(species.ID, breedName, null);
            }
            return breed;
        }
    }
}
