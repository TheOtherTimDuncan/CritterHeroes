using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Services;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Mappers;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using CritterHeroes.Web.Shared.Proxies.Configuration;
using Newtonsoft.Json;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.UnitTestHelpers;

namespace CH.RescueGroupsHelper.Importer
{
    public class CritterImporter
    {
        private Writer _writer;
        private string _path;
        private string _filePath;

        public CritterImporter(Writer writer)
        {
            this._writer = writer;
            _path = Path.Combine(UnitTestHelper.GetSolutionRoot(), ".vs", "Critters");
            _filePath = Path.Combine(_path, "critters.json");
        }

        public async Task ImportFromWebAsync(CheckedListBox.CheckedItemCollection checkedFileldNames)
        {
            NullEventPublisher publisher = new NullEventPublisher();

            DateTimeOffset lastUpdated;
            using (CritterBatchStorageContext critterStorage = new CritterBatchStorageContext(publisher))
            {
                lastUpdated = critterStorage.Critters.Max(x => x.RescueGroupsLastUpdated) ?? DateTimeOffset.MinValue;
            }

            _writer.WriteLine($"Last updated: {lastUpdated}");
            _writer.WriteLine();

            HttpClientProxy client = new HttpClientProxy(_writer);
            CritterSourceStorage source = new CritterSourceStorage(new RescueGroupsConfiguration(), client, publisher);

            source.Fields.ForEach(x => x.IsSelected = (x.Name == "animalID"));

            IEnumerable<SearchFilter> filters = source.Fields.Where(x => checkedFileldNames.Contains(x.Name)).Select(x =>
            {
                x.IsSelected = true;
                return new SearchFilter()
                {
                    FieldName = x.Name,
                    Operation = SearchFilterOperation.NotBlank
                };
            });

            bool isPartial = (filters.Count() != source.Fields.Count());

            IEnumerable<CritterSource> sources;
            if (isPartial)
            {
                source.FilterProcessing = string.Join(" or ", filters.Select((SearchFilter filter, int i) => i + 1));
                sources = await source.GetAllAsync(filters.ToArray());
            }
            else
            {
                SearchFilter filter = new SearchFilter()
                {
                    FieldName = "animalUpdatedDate",
                    Operation = SearchFilterOperation.GreaterThanOrEqual,
                    Criteria = lastUpdated.ToString("MM/dd/yyyy")
                };
                sources = await source.GetAllAsync(filter);
            }

            IEnumerable<SearchField> fields = source.Fields.Where(x => x.IsSelected);

            string json = File.ReadAllText(_filePath);
            IEnumerable<CritterSource> existing = JsonConvert.DeserializeObject<IEnumerable<CritterSource>>(json);

            IEnumerable<CritterSource> merged = MergeUpdatedWithExisting(existing, sources, fields, isPartial);

            File.WriteAllText(_filePath, JsonConvert.SerializeObject(merged, Formatting.Indented));

            await ImportCrittersAsync(sources, fields.Select(x => x.Name));
        }

        public async Task ImportFromFileAsync()
        {
            string json = File.ReadAllText(_filePath);
            IEnumerable<CritterSource> sources = JsonConvert.DeserializeObject<IEnumerable<CritterSource>>(json);
            IEnumerable<string> fieldNames = new CritterSourceStorage(new RescueGroupsConfiguration(), new HttpClientProxy(_writer), new NullEventPublisher()).Fields.Select(x => x.Name);
            await ImportCrittersAsync(sources, fieldNames);
        }

        private IEnumerable<CritterSource> MergeUpdatedWithExisting(IEnumerable<CritterSource> existing, IEnumerable<CritterSource> updated, IEnumerable<SearchField> fields, bool isPartial)
        {
            Type sourceType = typeof(CritterSource);
            IEnumerable<PropertyInfo> sourceProperties = sourceType.GetProperties();

            if (isPartial)
            {
                foreach (CritterSource original in existing)
                {
                    CritterSource source = updated.SingleOrDefault(x => x.ID == original.ID);
                    if (source != null)
                    {
                        foreach (PropertyInfo property in sourceProperties)
                        {
                            JsonPropertyAttribute attribute = property.GetCustomAttributes<JsonPropertyAttribute>().Single();

                            if (fields.Any(x => x.Name == attribute.PropertyName || x.SupportingFields.Contains(attribute.PropertyName)))
                            {
                                object value = property.GetValue(source);
                                property.SetValue(original, value);
                            }
                        }
                    }
                }
                return existing;
            }
            else
            {
                return existing.Where(x => !updated.Any(s => s.ID == x.ID))
                    .Concat(updated)
                    .OrderBy(x => x.ID);
            }
        }

        private async Task ImportCrittersAsync(IEnumerable<CritterSource> sources, IEnumerable<string> fieldNames)
        {
            _writer.WriteLine($"Importing {sources.Count()}");

            NullEventPublisher publisher = new NullEventPublisher();
            CritterMapper mapper = new CritterMapper();
            CritterPictureService pictureService = new CritterPictureService(new AzureService(new AzureConfiguration(), new ImporterOrganizationStateManager(), new AppConfiguration()));

            Guid organizationID = new Guid("71A22C0B-23FB-4FC0-96A8-792474C80953");

            using (CritterBatchStorageContext critterStorage = new CritterBatchStorageContext(publisher))
            {
                foreach (CritterSource source in sources)
                {
                    Critter critter = await critterStorage.Critters.FindByRescueGroupsIDAsync(source.ID);

                    CritterMapperContext context = new CritterMapperContext(source, critter, publisher)
                    {
                        Status = await GetCritterStatusAsync(critterStorage, source.StatusID, source.Status),
                        Breed = await GetBreedAsync(critterStorage, source.PrimaryBreedID, source.PrimaryBreed, source.Species),
                        Location = await GetLocationAsync(critterStorage, source.LocationID, source),
                        Foster = await GetFosterAsync(critterStorage, source.FosterContactID, source.FosterFirstName, source.FosterLastName, source.FosterEmail),
                        Color = await GetColorAsync(critterStorage, source.ColorID, source.Color)
                    };

                    if (critter == null)
                    {
                        critter = new Critter(source.Name, context.Status, context.Breed, organizationID, source.ID);
                        critterStorage.AddCritter(critter);
                        context.Target = critter;
                        _writer.WriteLine($"Added {source.ID} - {source.Name}");
                    }
                    else
                    {
                        _writer.WriteLine($"Updating {source.ID} - {source.Name}");
                        critter.WhenUpdated = DateTimeOffset.UtcNow;
                    }

                    mapper.MapSourceToTarget(context, fieldNames);

                    await critterStorage.SaveChangesAsync();

                    await ImportPicturesAsync(critterStorage, pictureService, context);
                }
            }

            _writer.WriteLine("Import complete.");
        }

        private async Task ImportPicturesAsync(CritterBatchStorageContext critterStorage, CritterPictureService pictureService, CritterMapperContext context)
        {
            if (!context.Source.PictureSources.IsNullOrEmpty())
            {
                foreach (CritterPictureSource pictureSource in context.Source.PictureSources)
                {
                    CritterPicture critterPicture = context.Target.Pictures.SingleOrDefault(x => x.Picture.Filename == pictureSource.Filename);
                    if (critterPicture == null)
                    {
                        string contentType = await ImportPictureAsync(pictureService, pictureSource.Url, context.Target.ID, pictureSource.Filename);
                        Picture picture = new Picture(pictureSource.Filename, pictureSource.Width, pictureSource.Height, pictureSource.FileSize, contentType)
                        {
                            RescueGroupsID = pictureSource.ID,
                            RescueGroupsCreated = DateTime.Parse(pictureSource.LastUpdated),
                            DisplayOrder = pictureSource.DisplayOrder,
                            SourceUrl = pictureSource.Url
                        };

                        if (
                            pictureSource.LargePicture != null
                            && pictureSource.LargePicture.Width != pictureSource.Width
                            && pictureSource.LargePicture.Height != pictureSource.Height
                            && !context.Target.Pictures.Any(x => x.Picture.Width == pictureSource.LargePicture.Width && x.Picture.Height == pictureSource.LargePicture.Height)
                        )
                        {
                            await ImportPictureAsync(pictureService, pictureSource.LargePicture.Url, context.Target.ID, pictureSource.LargePicture.Filename);
                            picture.AddChildPicture(pictureSource.LargePicture.Width, pictureSource.LargePicture.Height, pictureSource.LargePicture.FileSize, pictureSource.LargePicture.Filename);
                        }

                        if (!context.Target.Pictures.Any(x => x.Picture.Width == pictureSource.SmallPicture.Width && x.Picture.Height == pictureSource.SmallPicture.Height))
                        {
                            await ImportPictureAsync(pictureService, pictureSource.SmallPicture.Url, context.Target.ID, pictureSource.SmallPicture.Filename);
                            PictureChild pictureChild = picture.AddChildPicture(pictureSource.SmallPicture.Width, pictureSource.SmallPicture.Height, pictureSource.SmallPicture.FileSize, pictureSource.SmallPicture.Filename);
                        }

                        critterPicture = context.Target.AddPicture(picture);
                        _writer.WriteLine($"Added picture {pictureSource.Filename} for {context.Source.ID} - {context.Source.Name}");
                    }
                    else
                    {
                        critterPicture.Picture.SourceUrl = pictureSource.Url;
                        bool pictureExists = await pictureService.PictureExistsAsync(context.Target.ID, critterPicture.Picture.Filename);
                        if (!pictureExists)
                        {
                            await ImportPictureAsync(pictureService, pictureSource.Url, context.Target.ID, critterPicture.Picture.Filename);
                            _writer.WriteLine($"Replaced lost picture {pictureSource.Filename} for {context.Source.ID} - {context.Source.Name}");
                        }

                        foreach (PictureChild childPicture in critterPicture.Picture.ChildPictures)
                        {
                            pictureExists = await pictureService.PictureExistsAsync(context.Target.ID, childPicture.Filename);
                            if (!pictureExists)
                            {
                                if (pictureSource.LargePicture.Width == childPicture.Width && pictureSource.LargePicture.Height == childPicture.Height)
                                {
                                    await ImportPictureAsync(pictureService, pictureSource.LargePicture.Url, context.Target.ID, childPicture.Filename);
                                    _writer.WriteLine($"Replaced lost picture {pictureSource.Filename} for {childPicture.Width}x{childPicture.Height} for {context.Source.ID} - {context.Source.Name} not in target or source");
                                }
                                else if (pictureSource.SmallPicture.Width == childPicture.Width && pictureSource.SmallPicture.Height == childPicture.Height)
                                {
                                    await ImportPictureAsync(pictureService, pictureSource.SmallPicture.Url, context.Target.ID, childPicture.Filename);
                                    _writer.WriteLine($"Replaced lost picture {pictureSource.Filename} for {childPicture.Width}x{childPicture.Height} for {context.Source.ID} - {context.Source.Name} not in target or source");
                                }
                                else
                                {
                                    _writer.WriteLine($"Picture {pictureSource.Filename} for {childPicture.Width}x{childPicture.Height} for {context.Source.ID} - {context.Source.Name} not in target or source");
                                }
                            }
                        }

                        await critterStorage.SaveChangesAsync();
                    }
                }
            }
        }

        private async Task<string> ImportPictureAsync(CritterPictureService pictureService, string urlFrom, int critterID, string filename)
        {
            WebRequest pictureRequest = WebRequest.Create(urlFrom);
            using (WebResponse webResponse = await pictureRequest.GetResponseAsync())
            {
                using (Stream stream = webResponse.GetResponseStream())
                {
                    MemoryStream memStream = new MemoryStream();
                    stream.CopyTo(memStream);
                    memStream.Position = 0;

                    await pictureService.SavePictureAsync(memStream, critterID, filename, webResponse.ContentType);

                    string folder = Path.Combine(_path, critterID.ToString());
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    using (FileStream filestream = File.Create(Path.Combine(folder, filename)))
                    {
                        memStream.Position = 0;
                        memStream.CopyTo(filestream);
                    }
                }
                return webResponse.ContentType;
            }
        }

        private async Task<CritterStatus> GetCritterStatusAsync(CritterBatchStorageContext critterStorage, int? statusID, string status)
        {
            if (statusID == null)
            {
                return null;
            }

            CritterStatus critterStatus = await critterStorage.CritterStatus.FindByRescueGroupsIDAsync(statusID.Value);
            if (critterStatus == null)
            {
                _writer.WriteLine($"Added status {status}");
                critterStatus = new CritterStatus(status, status, statusID.Value);
            }
            return critterStatus;
        }

        private async Task<Breed> GetBreedAsync(CritterBatchStorageContext critterStorage, int? breedID, string breedName, string speciesName)
        {
            if (breedID == null)
            {
                return null;
            }

            Breed breed = await critterStorage.Breeds.FindByRescueGroupsIDAsync(breedID.Value);
            if (breed == null)
            {
                Species species = await critterStorage.Species.FindByNameAsync(speciesName);
                if (species == null)
                {
                    _writer.WriteLine($"Added species {speciesName}");
                    species = new Species(speciesName, speciesName, speciesName, null, null);
                }

                _writer.WriteLine($"Added breed {speciesName} - {breedName}");
                breed = new Breed(species.ID, breedName, breedID);
            }
            return breed;
        }

        private async Task<Person> GetFosterAsync(CritterBatchStorageContext critterStorage, int? fosterID, string firstName, string lastName, string email)
        {
            if (fosterID == null)
            {
                return null;
            }

            Person person = await critterStorage.People.FindByRescueGroupsIDAsync(fosterID.Value);

            if (person == null)
            {
                _writer.WriteLine($"Added person {firstName} {lastName}");
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

        private async Task<Location> GetLocationAsync(CritterBatchStorageContext critterStorage, int? locationID, CritterSource source)
        {
            if (locationID == null)
            {
                return null;
            }

            Location location = await critterStorage.Locations.FindByRescueGroupsIDAsync(locationID.Value);

            if (location == null)
            {
                _writer.WriteLine($"Added location {source.LocationName}");
                location = new Location(source.LocationName)
                {
                    RescueGroupsID = locationID
                };
            }

            location.Address = source.LocationAddress;
            location.City = source.LocationCity;
            location.State = source.LocationState;
            location.Zip = source.LocationZip;
            location.Phone = source.LocationPhone;
            location.Website = source.LocationUrl;

            return location;
        }

        private async Task<CritterColor> GetColorAsync(CritterBatchStorageContext critterStorage, int? colorID, string colorDescription)
        {
            if (colorID == null)
            {
                return null;
            }

            CritterColor color = await critterStorage.Colors.FindByRescueGroupsIDAsync(colorID.Value);

            if (color == null)
            {
                _writer.WriteLine($"Added color {colorDescription}");
                color = new CritterColor(colorDescription)
                {
                    RescueGroupsID = colorID
                };
            }

            return color;
        }
    }
}
