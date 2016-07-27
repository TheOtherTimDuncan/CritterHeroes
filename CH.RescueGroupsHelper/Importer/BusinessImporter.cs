using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Mappers;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using Newtonsoft.Json;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.UnitTestHelpers;

namespace CH.RescueGroupsHelper.Importer
{
    public class BusinessImporter : BaseContactImporter
    {
        private Writer _writer;
        private string _path;
        private IAppEventPublisher _publisher;

        public BusinessImporter(Writer writer)
        {
            this._writer = writer;
            this._path = Path.Combine(UnitTestHelper.GetSolutionRoot(), ".vs", "businesses.json");
            this._publisher = new NullEventPublisher();
        }

        public async Task ImportWebAsync()
        {
            BusinessSourceStorage sourceStorage = new BusinessSourceStorage(new RescueGroupsConfiguration(), new HttpClientProxy(_writer), _publisher);
            IEnumerable<BusinessSource> sources = await sourceStorage.GetAllAsync();

            if (!sources.IsNullOrEmpty())
            {
                File.WriteAllText(_path, JsonConvert.SerializeObject(sources, Formatting.Indented));
                await ImportData(sources, sourceStorage.Fields.Select(x => x.Name));
            }
        }

        public async Task ImportFileAsync()
        {
            string json = File.ReadAllText(_path);
            IEnumerable<BusinessSource> sources = JsonConvert.DeserializeObject<IEnumerable<BusinessSource>>(json);
            BusinessSourceStorage sourceStorage = new BusinessSourceStorage(new RescueGroupsConfiguration(), new HttpClientProxy(_writer), _publisher);
            await ImportData(sources, sourceStorage.Fields.Select(x => x.Name));
        }

        private async Task ImportData(IEnumerable<BusinessSource> sources, IEnumerable<string> fieldNames)
        {
            IEnumerable<PhoneType> phoneTypes = await GetPhoneTypesAsync();

            BusinessMapper mapper = new BusinessMapper();

            using (SqlStorageContext<Business> storageBusinesses = new SqlStorageContext<Business>(_publisher))
            using (SqlStorageContext<Group> storageGroups = new SqlStorageContext<Group>(_publisher))
            {
                foreach (BusinessSource source in sources)
                {
                    Business business = await storageBusinesses.Entities.FindByRescueGroupsIDAsync(source.ID);
                    if (business == null)
                    {
                        business = new Business()
                        {
                            RescueGroupsID = source.ID
                        };
                        storageBusinesses.Add(business);
                        _writer.WriteLine($"Added {source.ID} - {source.Company}");
                    }
                    else
                    {
                        _writer.WriteLine($"Updated {source.ID} - {source.Company}");
                    }

                    await AddOrUpdateGroupsAsync(storageGroups, source.GroupNames);

                    BusinessMapperContext context = new BusinessMapperContext(source, business, _publisher)
                    {
                        PhoneTypes = phoneTypes,
                        Groups = await storageGroups.GetAllAsync()
                    };

                    mapper.MapSourceToTarget(context, fieldNames);

                    await storageBusinesses.SaveChangesAsync();
                }
            }
        }
    }
}
