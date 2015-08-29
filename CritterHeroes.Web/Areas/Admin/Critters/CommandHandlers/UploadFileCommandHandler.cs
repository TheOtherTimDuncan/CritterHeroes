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
    public class UploadFileCommandHandler : IAsyncCommandHandler<UploadFileCommand>
    {
        private ISqlStorageContext<Critter> _critterStorage;
        private ISqlStorageContext<CritterStatus> _statusStorage;
        private ISqlStorageContext<Breed> _breedStorage;
        private ISqlStorageContext<Species> _speciesStorage;

        public UploadFileCommandHandler(ISqlStorageContext<Critter> critterStorage, ISqlStorageContext<CritterStatus> statusStorage, ISqlStorageContext<Breed> breedStorage, ISqlStorageContext<Species> speciesStorage)
        {
            this._critterStorage = critterStorage;
            this._statusStorage = statusStorage;
            this._breedStorage = breedStorage;
            this._speciesStorage = speciesStorage;
        }

        public async Task<CommandResult> ExecuteAsync(UploadFileCommand command)
        {
            using (StreamReader reader = new StreamReader(command.File.InputStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    CritterSource critterSource = JsonConvert.DeserializeObject<CritterSource>(line);

                    CritterStatus status = await GetCritterStatus(critterSource.Status);
                    Breed breed = await GetBreed(critterSource.PrimaryBreed, critterSource.Species);

                    Critter critter = await _critterStorage.Entities.FindByRescueGroupsIDAsync(critterSource.AnimalID);
                    if (critter == null)
                    {
                        critter = new Critter(status, critterSource.Name, breed, critterSource.AnimalID);
                        _critterStorage.Add(critter);
                    }
                    else
                    {
                        critter.Name = critterSource.Name;

                        if (critter.BreedID != breed.ID)
                        {
                            critter.ChangeBreed(breed.ID);
                        }

                        if (critter.StatusID != status.ID)
                        {
                            critter.ChangeStatus(status.ID);
                        }
                    }

                    critter.Sex = critterSource.Sex;
                    critter.WhenUpdated = DateTimeOffset.UtcNow;
                }
            }

            return CommandResult.Success();
        }

        private async Task<CritterStatus> GetCritterStatus(string status)
        {
            CritterStatus critterStatus = await _statusStorage.FindByNameAsync(status);
            if (critterStatus == null)
            {
                critterStatus = new CritterStatus(status, status);
                _statusStorage.Add(critterStatus);
                await _statusStorage.SaveChangesAsync();
            }
            return critterStatus;
        }

        private async Task<Breed> GetBreed(string breedName, string speciesName)
        {
            Breed breed = await _breedStorage.Entities.FindByNameAsync(breedName);
            if (breed == null)
            {
                Species species = await _speciesStorage.Entities.FindByNameAsync(speciesName);
                if (species == null)
                {
                    species = new Species(speciesName, speciesName, speciesName, null, null);
                    _speciesStorage.Add(species);
                    await _speciesStorage.SaveChangesAsync();
                }

                breed = new Breed(species.ID, breedName, null);
                _breedStorage.Add(breed);
                await _breedStorage.SaveChangesAsync();
            }
            return breed;
        }
    }
}
