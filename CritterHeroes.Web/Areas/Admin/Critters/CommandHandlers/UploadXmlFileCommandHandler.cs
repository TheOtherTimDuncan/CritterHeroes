using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using CritterHeroes.Web.Areas.Admin.Critters.Commands;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using TOTD.Utility.EnumerableHelpers;

namespace CritterHeroes.Web.Areas.Admin.Critters.CommandHandlers
{
    public class UploadXmlFileCommandHandler : IAsyncCommandHandler<UploadXmlFileCommand>
    {
        private ICritterBatchSqlStorageContext _critterStorage;
        private IStateManager<OrganizationContext> _stateManager;

        public UploadXmlFileCommandHandler(ICritterBatchSqlStorageContext critterStorage, IStateManager<OrganizationContext> stateManager)
        {
            this._critterStorage = critterStorage;
            this._stateManager = stateManager;
        }

        public async Task<CommandResult> ExecuteAsync(UploadXmlFileCommand command)
        {
            OrganizationContext orgContext = _stateManager.GetContext();

            using (XmlReader reader = XmlReader.Create(command.File.InputStream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CritterXml));
                var data = (CritterXml)serializer.Deserialize(reader);

                if (data != null && !data.Critters.IsNullOrEmpty())
                {
                    foreach (CritterExport critterExport in data.Critters)
                    {
                        CritterStatus status = await GetCritterStatus(critterExport.Status);
                        Breed breed = await GetBreed(critterExport.Breed, critterExport.Species);

                        Critter critter = await _critterStorage.Critters.FindByRescueGroupsIDAsync(critterExport.ID);
                        if (critter == null)
                        {
                            critter = new Critter(critterExport.Name, status, breed, orgContext.OrganizationID, critterExport.ID);
                            _critterStorage.AddCritter(critter);
                        }
                        else
                        {
                            critter.Name = critterExport.Name;

                            if (critter.BreedID != breed.ID)
                            {
                                critter.ChangeBreed(breed);
                            }

                            if (critter.StatusID != status.ID)
                            {
                                critter.ChangeStatus(status);
                            }
                        }

                        critter.Sex = critterExport.Sex;
                        critter.RescueID = critterExport.RescueID;

                        if (!critterExport.LastUpdated.IsNullOrEmpty())
                        {
                            critter.RescueGroupsLastUpdated = DateTime.Parse(critterExport.LastUpdated);
                        }

                        critter.WhenUpdated = DateTimeOffset.UtcNow;

                        await _critterStorage.SaveChangesAsync();
                    };
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
