using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Critters.Commands;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using Microsoft.VisualBasic.FileIO;

namespace CritterHeroes.Web.Areas.Admin.Critters.CommandHandlers
{
    public class UploadCsvFileCommandHandler : IAsyncCommandHandler<UploadCsvFileCommand>
    {
        private ICritterBatchSqlStorageContext _critterStorage;

        public UploadCsvFileCommandHandler(ICritterBatchSqlStorageContext critterStorage)
        {
            this._critterStorage = critterStorage;
        }

        public async Task<CommandResult> ExecuteAsync(UploadCsvFileCommand command)
        {
            using (TextFieldParser parser = new TextFieldParser(command.File.InputStream))
            {
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;

                // Skip header
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    string statusName = fields[2];
                    string speciesName = fields[3];
                    string breedName = fields[4];

                    CritterStatus status = await GetCritterStatus(statusName);
                    Breed breed = await GetBreed(breedName, speciesName);

                    int critterID = int.Parse(fields[0]);
                    string critterName = fields[1];

                    Critter critter = await _critterStorage.Critters.FindByRescueGroupsIDAsync(critterID);
                    if (critter == null)
                    {
                        critter = new Critter(status, critterName, breed, critterID);
                        _critterStorage.AddCritter(critter);
                    }
                    else
                    {
                        critter.Name = critterName;

                        if (critter.BreedID != breed.ID)
                        {
                            critter.ChangeBreed(breed);
                        }

                        if (critter.StatusID != status.ID)
                        {
                            critter.ChangeStatus(status);
                        }
                    }

                    critter.Sex = fields[10];
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
            Breed breed = await _critterStorage.Breeds.FindBySpeciesAndNameAsync(speciesName, breedName);
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
