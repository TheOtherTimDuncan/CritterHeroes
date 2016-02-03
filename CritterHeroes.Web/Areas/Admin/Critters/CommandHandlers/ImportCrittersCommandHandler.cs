using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Critters.Commands;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Logging;
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
        private ICritterLogger _logger;

        public ImportCrittersCommandHandler(ICritterBatchSqlStorageContext critterStorage, IStateManager<OrganizationContext> stateManager, IRescueGroupsStorageContext<CritterSearchResult> sourceStorage, ICritterPictureService pictureService, ICritterLogger logger)
        {
            this._critterStorage = critterStorage;
            this._stateManager = stateManager;
            this._sourceStorage = sourceStorage;
            this._pictureService = pictureService;
            this._logger = logger;
        }

        public async Task<CommandResult> ExecuteAsync(ImportCrittersCommand command)
        {
            IEnumerable<CritterStatus> statuses = await _critterStorage.CritterStatus.ToListAsync();

            foreach (CritterStatus status in statuses)
            {
                SearchFilter filter = new SearchFilter()
                {
                    FieldName = "animalStatusID",
                    Operation = SearchFilterOperation.Equal,
                    Criteria = status.RescueGroupsID
                };

                IEnumerable<CritterSearchResult> sources = await _sourceStorage.GetAllAsync(filter);

                OrganizationContext orgContext = _stateManager.GetContext();

                foreach (CritterSearchResult source in sources)
                {
                    CritterStatus critterStatus = await GetCritterStatusAsync(source.StatusID, source.Status);
                    Breed breed = await GetBreedAsync(source.PrimaryBreedID, source.PrimaryBreed, source.Species);

                    Critter critter = await _critterStorage.Critters.FindByRescueGroupsIDAsync(source.ID);
                    if (critter == null)
                    {
                        critter = new Critter(source.Name, critterStatus, breed, orgContext.OrganizationID, source.ID);
                        _critterStorage.AddCritter(critter);
                        _logger.LogAction("Added {CritterID} - {CritterName}", source.ID, source.Name);
                    }
                    else
                    {
                        critter.Name = source.Name;

                        if (critter.BreedID != breed.ID)
                        {
                            _logger.LogAction("Changed breed from {OldBreed} to {NewBreed} for {CrittterID} - {CritterName}", critter.Breed.IfNotNull(x => x.BreedName, "not set"), breed.BreedName, source.ID, source.Name);
                            critter.ChangeBreed(breed);
                        }

                        if (critter.StatusID != critterStatus.ID)
                        {
                            _logger.LogAction("Changed status from {OldStatus} to {NewStatus} for {CrittterID} - {CritterName}", critter.Status.IfNotNull(x => x.Name, "not set"), critterStatus.Name, source.ID, source.Name);
                            critter.ChangeStatus(critterStatus);
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
                            _logger.LogAction("Changed foster from {OldFoster} to {NewFoster} for {CrittterID} - {CritterName}", critter.Foster.IfNotNull(x => x.FirstName, "not set"), person.FirstName, source.ID, source.Name);
                            critter.ChangeFoster(person);
                        }
                    }
                    else if (critter.FosterID != null)
                    {
                        _logger.LogAction("Removed foster {OldFoster} for {CrittterID} - {CritterName}", critter.Foster.IfNotNull(x => x.FirstName, "not set"), source.ID, source.Name);
                        critter.RemoveFoster();
                    }

                    if (!source.LocationID.IsNullOrEmpty())
                    {
                        Location location = await GetLocationAsync(source.LocationID, source.LocationName);

                        if (critter.LocationID != location.ID)
                        {
                            _logger.LogAction("Changed location from {OldLocation} to {NewLocation} for {CrittterID} - {CritterName}", critter.Location.IfNotNull(x => x.Name, "not set"), location.Name, source.ID, source.Name);
                            critter.ChangeLocation(location);
                        }
                    }
                    else if (critter.LocationID != null)
                    {
                        _logger.LogAction("Removed location {OldLocation} for {CrittterID} - {CritterName}", critter.Location.IfNotNull(x => x.Name, "not set"), source.ID, source.Name);
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
                                _logger.LogAction("Added picture {Filename} for {CritterID} - {CritterName}", pictureSource.Filename, source.ID, source.Name);
                            }
                        }
                    }

                    await _critterStorage.SaveChangesAsync();
                }
            }

            command.Messages = _logger.Messages;

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
