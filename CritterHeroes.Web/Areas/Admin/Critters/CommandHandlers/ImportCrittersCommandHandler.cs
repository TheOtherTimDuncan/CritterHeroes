using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.Misc;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Areas.Admin.Critters.CommandHandlers
{
    public class ImportCrittersCommandHandler : IAsyncCommandHandler<ImportCrittersCommand>
    {
        private IRescueGroupsStorageContext<CritterSearchResult> _sourceStorage;
        private ICritterBatchSqlStorageContext _critterStorage;
        private IStateManager<OrganizationContext> _stateManager;
        private ICritterPictureService _pictureService;

        private List<string> messages = new List<string>();

        public ImportCrittersCommandHandler(ICritterBatchSqlStorageContext critterStorage, IStateManager<OrganizationContext> stateManager, IRescueGroupsStorageContext<CritterSearchResult> sourceStorage, ICritterPictureService pictureService)
        {
            this._critterStorage = critterStorage;
            this._stateManager = stateManager;
            this._sourceStorage = sourceStorage;
            this._pictureService = pictureService;

            messages = new List<string>();
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
                    messages.Add($"{source.Name} - {source.ID} - Added");
                }
                else
                {
                    critter.Name = source.Name;

                    if (critter.BreedID != breed.ID)
                    {
                        messages.Add($"{source.Name} - {source.ID} - Changed breed from {critter.Breed.IfNotNull(x => x.BreedName, "not set")} to {breed.BreedName}");
                        critter.ChangeBreed(breed);
                    }

                    if (critter.StatusID != status.ID)
                    {
                        messages.Add($"{source.Name} - {source.ID} - Changed breed from {critter.Status.IfNotNull(x => x.Name, "not set")} to {status.Name}");
                        critter.ChangeStatus(status);
                    }
                }

                critter.Sex = source.Sex;
                critter.WhenUpdated = DateTimeOffset.UtcNow;
                critter.RescueID = source.RescueID;

                if (!source.FosterContactID.IsNullOrEmpty())
                {
                    Person person = await GetFosterAsync(source.FosterContactID, source.FosterFirstName, source.FosterLastName, source.FosterEmail);

                    if (critter.FosterID != person.ID)
                    {
                        messages.Add($"{source.Name} - {source.ID} - Changed foster from {critter.Foster.IfNotNull(x => x.FirstName, "not set")} to {person.FirstName}");
                        critter.ChangeFoster(person);
                    }
                }
                else if (critter.FosterID != null)
                {
                    messages.Add($"{source.Name} - {source.ID} - Removed foster {critter.Foster.IfNotNull(x => x.FirstName, "not set")}");
                    critter.RemoveFoster();
                }

                if (!source.LocationID.IsNullOrEmpty())
                {
                    Location location = await GetLocationAsync(source.LocationID, source.LocationName);

                    if (critter.LocationID != location.ID)
                    {
                        messages.Add($"{source.Name} - {source.ID} - Changed location from {critter.Location.IfNotNull(x => x.Name, "not set")} to {location.Name}");
                        critter.ChangeLocation(location);
                    }
                }
                else if (critter.LocationID != null)
                {
                    messages.Add($"{source.Name} - {source.ID} - Removed location {critter.Location.IfNotNull(x => x.Name, "not set")}");
                    critter.RemoveLocation();
                }

                if (!source.LastUpdated.IsNullOrEmpty())
                {
                    critter.RescueGroupsLastUpdated = DateTime.Parse(source.LastUpdated);
                }

                if (!source.Created.IsNullOrEmpty())
                {
                    critter.RescueGroupsCreated = DateTime.Parse(source.Created);
                }

                // Save changes before transferring pictures since we'll need Critter.ID 
                await _critterStorage.SaveChangesAsync();

                if (!source.PictureSources.IsNullOrEmpty())
                {
                    foreach (CritterPictureSource pictureSource in source.PictureSources)
                    {
                        if (!critter.Pictures.Any(x => x.Picture.Filename == pictureSource.Filename))
                        {
                            string contentType = await ImportPicture(pictureSource.Url, critter.ID, pictureSource.Filename);
                            Picture picture = new Picture(pictureSource.Filename, pictureSource.Width, pictureSource.Height, pictureSource.FileSize, contentType)
                            {
                                RescueGroupsID = pictureSource.ID,
                                RescueGroupsCreated = DateTime.Parse(pictureSource.LastUpdated),
                                DisplayOrder = pictureSource.DisplayOrder
                            };

                            if (
                                pictureSource.LargePicture != null
                                && pictureSource.LargePicture.Width != pictureSource.Width
                                && pictureSource.LargePicture.Height != pictureSource.Height
                                && !critter.Pictures.Any(x => x.Picture.Width == pictureSource.LargePicture.Width && x.Picture.Height == pictureSource.LargePicture.Height)
                            )
                            {
                                await ImportPicture(pictureSource.LargePicture.Url, critter.ID, pictureSource.LargePicture.Filename);
                                picture.AddChildPicture(pictureSource.LargePicture.Width, pictureSource.LargePicture.Height, pictureSource.LargePicture.FileSize, pictureSource.LargePicture.Filename);
                            }

                            if (!critter.Pictures.Any(x => x.Picture.Width == pictureSource.SmallPicture.Width && x.Picture.Height == pictureSource.SmallPicture.Height))
                            {
                                await ImportPicture(pictureSource.SmallPicture.Url, critter.ID, pictureSource.SmallPicture.Filename);
                                PictureChild pictureChild = picture.AddChildPicture(pictureSource.SmallPicture.Width, pictureSource.SmallPicture.Height, pictureSource.SmallPicture.FileSize, pictureSource.SmallPicture.Filename);
                            }

                            CritterPicture critterPicture = critter.AddPicture(picture);
                            messages.Add($"{source.Name} - {source.ID} - Added picture {pictureSource.Filename}");
                        }
                    }
                }

                await _critterStorage.SaveChangesAsync();
            }

            command.Messages = messages;

            return CommandResult.Success();
        }

        private async Task<string> ImportPicture(string urlFrom, int critterID, string filename)
        {
            WebRequest pictureRequest = WebRequest.Create(urlFrom);
            using (WebResponse webResponse = await pictureRequest.GetResponseAsync())
            {
                using (Stream stream = webResponse.GetResponseStream())
                {
                    await _pictureService.SavePictureAsync(stream, critterID, filename, webResponse.ContentType);
                }
                return webResponse.ContentType;
            }
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

        private async Task<Location> GetLocationAsync(string locationID, string locationName)
        {
            Location location = await _critterStorage.Locations.FindByRescueGroupsIDAsync(locationID);

            if (location == null)
            {
                location = new Location(locationName)
                {
                    RescueGroupsID = locationID
                };
            }

            return location;
        }
    }
}
