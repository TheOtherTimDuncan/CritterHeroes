using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Mappers;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using CritterHeroes.Web.Models.LogEvents;
using Newtonsoft.Json.Linq;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Features.Admin.Critters.Commands
{
    public class CrittersImportCommandHandler : IAsyncCommandHandler<CritterImportModel>
    {
        private IRescueGroupsStorageContext<CritterSource> _sourceStorage;
        private ICritterBatchSqlStorageContext _critterStorage;
        private IStateManager<OrganizationContext> _stateManager;
        private ICritterPictureService _pictureService;
        private IAppEventPublisher _publisher;

        private List<string> _messages;

        public CrittersImportCommandHandler(ICritterBatchSqlStorageContext critterStorage, IStateManager<OrganizationContext> stateManager, IRescueGroupsStorageContext<CritterSource> sourceStorage, ICritterPictureService pictureService, IAppEventPublisher publisher)
        {
            this._critterStorage = critterStorage;
            this._stateManager = stateManager;
            this._sourceStorage = sourceStorage;
            this._pictureService = pictureService;
            this._publisher = publisher;

            this._messages = new List<string>();

            _publisher.Subscribe<CritterLogEvent>(HandleCritterLogEvent);
            _publisher.Subscribe<HistoryLogEvent>(HandleHistoryLogEvent);
        }

        public async Task<CommandResult> ExecuteAsync(CritterImportModel command)
        {
            if (command.FieldNames.Count() != _sourceStorage.Fields.Count())
            {
                await ImportPartial(command.FieldNames);
            }
            else
            {
                await ImportAll();
            }

            command.Messages = _messages;

            return CommandResult.Success();
        }

        public void HandleCritterLogEvent(CritterLogEvent appEvent)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("\"");
            builder.Append(appEvent.MessageTemplate);
            builder.Append("\"");

            foreach (var value in appEvent.MessageValues)
            {
                builder.Append(", ");
                builder.Append(value);
            }

            _messages.Add(builder.ToString());
        }

        public void HandleHistoryLogEvent(HistoryLogEvent appEvent)
        {
            var context = (HistoryLogEvent.HistoryContext)appEvent.Context;

            Dictionary<string, HistoryValue> values = new Dictionary<string, HistoryValue>();

            JObject before = JObject.Parse(context.Before);
            foreach (JProperty property in before.Properties())
            {
                values.Add(property.Name, new HistoryValue()
                {
                    Before = property.Value.Value<object>()
                });
            }

            JObject after = JObject.Parse(context.After);
            foreach (JProperty property in after.Properties())
            {
                HistoryValue historyValue;
                if (!values.TryGetValue(property.Name, out historyValue))
                {
                    historyValue = new HistoryValue();
                    values.Add(property.Name, historyValue);
                }
                historyValue.After = property.Value.Value<object>();
            }

            StringBuilder builder = new StringBuilder();

            builder.Append("\"");
            builder.Append(appEvent.MessageTemplate);
            builder.Append("\"");

            foreach (var value in appEvent.MessageValues)
            {
                builder.Append(", ");
                builder.Append(value);
            }

            builder.Append("<table>");

            foreach (var keyValue in values)
            {
                builder.Append("<tr>");

                builder.Append("<td>");
                builder.Append(keyValue.Key);
                builder.Append("</td>");

                builder.Append("<td>");
                builder.Append(keyValue.Value.Before);
                builder.Append("</td>");

                builder.Append("<td>");
                builder.Append(keyValue.Value.After);
                builder.Append("</td>");

                builder.Append("</tr>");
            }

            builder.Append("</table>");

            _messages.Add(builder.ToString());
        }

        private async Task ImportPartial(IEnumerable<string> fieldNames, SearchFilter additionalFilter = null)
        {
            List<SearchFilter> filters = new List<SearchFilter>();

            if (additionalFilter != null)
            {
                filters.Add(additionalFilter);
            }

            filters.AddRange(_sourceStorage.Fields.NullSafeSelect(x =>
            {
                x.IsSelected = (x.Name == "animalID" || fieldNames.Contains(x.Name));
                return new SearchFilter()
                {
                    FieldName = x.Name,
                    Criteria = SearchFilterOperation.NotBlank
                };
            }));

            if (additionalFilter != null)
            {
                _sourceStorage.FilterProcessing = "0 and (" + string.Join(" or ", filters.Select((SearchFilter filter, int i) => i + 2)) + ")";
            }
            else
            {
                _sourceStorage.FilterProcessing = string.Join(" or ", filters.Select((SearchFilter filter, int i) => i + 1));
            }

            if (_sourceStorage.Fields.Any(x => x.IsSelected && x.Name == "animalDescription"))
            {
                _sourceStorage.ResultLimit = 10;
            }
            else if (_sourceStorage.Fields.Any(x => x.IsSelected && x.Name == "animalPictures"))
            {
                _sourceStorage.ResultLimit = 5;
            }

            CritterMapper mapper = new CritterMapper();

            IEnumerable<CritterSource> sources = await _sourceStorage.GetAllAsync(filters.ToArray());
            _messages.Add($"Found {sources.Count()}");

            foreach (CritterSource source in sources)
            {
                Critter critter = await _critterStorage.Critters.FindByRescueGroupsIDAsync(source.ID);

                CritterMapperContext context = new CritterMapperContext(source, critter, _publisher)
                {
                    Status = await GetCritterStatusAsync(source.StatusID, source.Status),
                    Breed = await GetBreedAsync(source.PrimaryBreedID, source.PrimaryBreed, source.Species),
                    Location = await GetLocationAsync(source.LocationID, source.LocationName),
                    Foster = await GetFosterAsync(source.FosterContactID, source.FosterFirstName, source.FosterLastName, source.FosterEmail),
                    Color = await GetColorAsync(source.ColorID, source.Color)
                };

                OrganizationContext orgContext = _stateManager.GetContext();

                if (critter == null)
                {
                    critter = new Critter(source.Name, context.Status, context.Breed, orgContext.OrganizationID, source.ID);
                    _critterStorage.AddCritter(critter);
                    context.Target = critter;
                    _publisher.Publish(CritterLogEvent.Action("Added {CritterID} - {CritterName}", source.ID, source.Name));
                }
                else
                {
                    critter.WhenUpdated = DateTimeOffset.UtcNow;
                }

                mapper.MapSourceToTarget(context, fieldNames.ToArray());

                // Save changes before transferring pictures since we'll need Critter.ID 
                await _critterStorage.SaveChangesAsync();

                await ImportPictures(context);
            }
        }

        private async Task ImportAll()
        {
            DateTimeOffset lastUpdated = _critterStorage.Critters.Max(x => x.RescueGroupsLastUpdated) ?? DateTimeOffset.MinValue;

            _messages.Add($"Importing updated since {lastUpdated}...");

            SearchFilter filter = new SearchFilter()
            {
                FieldName = "animalUpdatedDate",
                Operation = SearchFilterOperation.GreaterThanOrEqual,
                Criteria = lastUpdated.ToString("MM/dd/yyyy")
            };

            // Exclude pictures and descriptions since they'll max out the size of the request for logging
            _sourceStorage.Fields.Single(x => x.Name == "animalPictures").IsSelected = false;
            _sourceStorage.Fields.Single(x => x.Name == "animalDescription").IsSelected = false;

            CritterMapper mapper = new CritterMapper();

            OrganizationContext orgContext = _stateManager.GetContext();

            IEnumerable<CritterSource> sources = await _sourceStorage.GetAllAsync(filter);
            _messages.Add($"Found {sources.Count()}");

            foreach (CritterSource source in sources)
            {
                Critter critter = await _critterStorage.Critters.FindByRescueGroupsIDAsync(source.ID);

                CritterMapperContext context = new CritterMapperContext(source, critter, _publisher)
                {
                    Status = await GetCritterStatusAsync(source.StatusID, source.Status),
                    Breed = await GetBreedAsync(source.PrimaryBreedID, source.PrimaryBreed, source.Species),
                    Location = await GetLocationAsync(source.LocationID, source.LocationName),
                    Foster = await GetFosterAsync(source.FosterContactID, source.FosterFirstName, source.FosterLastName, source.FosterEmail),
                    Color = await GetColorAsync(source.ColorID, source.Color)
                };

                if (critter == null)
                {
                    critter = new Critter(source.Name, context.Status, context.Breed, orgContext.OrganizationID, source.ID);
                    _critterStorage.AddCritter(critter);
                    context.Target = critter;
                    _publisher.Publish(CritterLogEvent.Action("Added {CritterID} - {CritterName}", source.ID, source.Name));
                }
                else
                {
                    critter.WhenUpdated = DateTimeOffset.UtcNow;
                }

                mapper.MapSourceToTarget(context, _sourceStorage.Fields.Where(x => x.IsSelected).Select(x => x.Name).ToArray());

                await _critterStorage.SaveChangesAsync();
            }

            // Import descriptions
            await ImportPartial(new[] { "animalDescription" }, filter);

            // Import pictures
            await ImportPartial(new[] { "animalPictures" }, filter);
        }

        private async Task ImportPictures(CritterMapperContext context)
        {
            if (!context.Source.PictureSources.IsNullOrEmpty())
            {
                foreach (CritterPictureSource pictureSource in context.Source.PictureSources)
                {
                    CritterPicture critterPicture = context.Target.Pictures.SingleOrDefault(x => x.Picture.Filename == pictureSource.Filename);
                    if (critterPicture == null)
                    {
                        string contentType = await ImportPicture(pictureSource.Url, context.Target.ID, pictureSource.Filename);
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
                            && !context.Target.Pictures.Any(x => x.Picture.Width == pictureSource.LargePicture.Width && x.Picture.Height == pictureSource.LargePicture.Height)
                        )
                        {
                            await ImportPicture(pictureSource.LargePicture.Url, context.Target.ID, pictureSource.LargePicture.Filename);
                            picture.AddChildPicture(pictureSource.LargePicture.Width, pictureSource.LargePicture.Height, pictureSource.LargePicture.FileSize, pictureSource.LargePicture.Filename);
                        }

                        if (!context.Target.Pictures.Any(x => x.Picture.Width == pictureSource.SmallPicture.Width && x.Picture.Height == pictureSource.SmallPicture.Height))
                        {
                            await ImportPicture(pictureSource.SmallPicture.Url, context.Target.ID, pictureSource.SmallPicture.Filename);
                            PictureChild pictureChild = picture.AddChildPicture(pictureSource.SmallPicture.Width, pictureSource.SmallPicture.Height, pictureSource.SmallPicture.FileSize, pictureSource.SmallPicture.Filename);
                        }

                        critterPicture = context.Target.AddPicture(picture);
                        _publisher.Publish(CritterLogEvent.Action("Added picture {Filename} for {CritterID} - {CritterName}", pictureSource.Filename, context.Source.ID, context.Source.Name));
                    }
                    else
                    {
                        bool pictureExists = await _pictureService.PictureExistsAsync(context.Target.ID, critterPicture.Picture.Filename);
                        if (!pictureExists)
                        {
                            await ImportPicture(pictureSource.Url, context.Target.ID, critterPicture.Picture.Filename);
                            _publisher.Publish(CritterLogEvent.Action("Replaced lost picture {Filename} for {CritterID} - {CritterName}", pictureSource.Filename, context.Source.ID, context.Source.Name));
                        }

                        foreach (PictureChild childPicture in critterPicture.Picture.ChildPictures)
                        {
                            pictureExists = await _pictureService.PictureExistsAsync(context.Target.ID, childPicture.Filename);
                            if (!pictureExists)
                            {
                                if (pictureSource.LargePicture.Width == childPicture.Width && pictureSource.LargePicture.Height == childPicture.Height)
                                {
                                    await ImportPicture(pictureSource.LargePicture.Url, context.Target.ID, childPicture.Filename);
                                    _publisher.Publish(CritterLogEvent.Action("Replaced lost picture {Filename} for {Width}x{Height} for {CritterID} - {CritterName} not in target or source", pictureSource.Filename, childPicture.Width, childPicture.Height, context.Source.ID, context.Source.Name));
                                }
                                else if (pictureSource.SmallPicture.Width == childPicture.Width && pictureSource.SmallPicture.Height == childPicture.Height)
                                {
                                    await ImportPicture(pictureSource.SmallPicture.Url, context.Target.ID, childPicture.Filename);
                                    _publisher.Publish(CritterLogEvent.Action("Replaced lost picture {Filename} for {Width}x{Height} for {CritterID} - {CritterName} not in target or source", pictureSource.Filename, childPicture.Width, childPicture.Height, context.Source.ID, context.Source.Name));
                                }
                                else
                                {
                                    _publisher.Publish(CritterLogEvent.Action("Picture {Filename} for {Width}x{Height} for {CritterID} - {CritterName} not in target or source", pictureSource.Filename, childPicture.Width, childPicture.Height, context.Source.ID, context.Source.Name));
                                }
                            }
                        }

                    }
                }

                await _critterStorage.SaveChangesAsync();
            }
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

        private async Task<CritterStatus> GetCritterStatusAsync(int? statusID, string status)
        {
            if (statusID == null)
            {
                return null;
            }

            CritterStatus critterStatus = await _critterStorage.CritterStatus.FindByRescueGroupsIDAsync(statusID.Value);
            if (critterStatus == null)
            {
                critterStatus = new CritterStatus(status, status, statusID.Value);
            }
            return critterStatus;
        }

        private async Task<Breed> GetBreedAsync(int? breedID, string breedName, string speciesName)
        {
            if (breedID == null)
            {
                return null;
            }

            Breed breed = await _critterStorage.Breeds.FindByRescueGroupsIDAsync(breedID.Value);
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

        private async Task<Person> GetFosterAsync(int? fosterID, string firstName, string lastName, string email)
        {
            if (fosterID == null)
            {
                return null;
            }

            Person person = await _critterStorage.People.FindByRescueGroupsIDAsync(fosterID.Value);

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

        private async Task<Location> GetLocationAsync(int? locationID, string locationName)
        {
            if (locationID == null)
            {
                return null;
            }

            Location location = await _critterStorage.Locations.FindByRescueGroupsIDAsync(locationID.Value);

            if (location == null)
            {
                location = new Location(locationName)
                {
                    RescueGroupsID = locationID
                };
            }

            return location;
        }

        private async Task<CritterColor> GetColorAsync(int? colorID, string colorDescription)
        {
            if (colorID == null)
            {
                return null;
            }

            CritterColor color = await _critterStorage.Colors.FindByRescueGroupsIDAsync(colorID.Value);

            if (color == null)
            {
                color = new CritterColor(colorDescription)
                {
                    RescueGroupsID = colorID
                };
            }

            return color;
        }

        private class HistoryValue
        {
            public object Before
            {
                get;
                set;
            }

            public object After
            {
                get;
                set;
            }
        }
    }
}
