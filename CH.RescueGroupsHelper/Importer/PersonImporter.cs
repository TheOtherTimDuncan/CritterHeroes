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
    public class PersonImporter : BaseContactImporter
    {
        private Writer _writer;
        private string _path;
        private IAppEventPublisher _publisher;

        public PersonImporter(Writer writer)
        {
            this._writer = writer;
            this._path = Path.Combine(UnitTestHelper.GetSolutionRoot(), ".vs", "people.json");
            this._publisher = new NullEventPublisher();
        }

        public async Task ImportWebAsync()
        {
            PersonSourceStorage sourceStorage = new PersonSourceStorage(new RescueGroupsConfiguration(), new HttpClientProxy(_writer), _publisher);
            IEnumerable<PersonSource> sources = await sourceStorage.GetAllAsync();

            if (!sources.IsNullOrEmpty())
            {
                File.WriteAllText(_path, JsonConvert.SerializeObject(sources, Formatting.Indented));
                await ImportData(sources, sourceStorage.Fields.Select(x => x.Name));
            }
        }

        public async Task ImportFileAsync()
        {
            string json = File.ReadAllText(_path);
            IEnumerable<PersonSource> sources = JsonConvert.DeserializeObject<IEnumerable<PersonSource>>(json);
            PersonSourceStorage sourceStorage = new PersonSourceStorage(new RescueGroupsConfiguration(), new HttpClientProxy(_writer), _publisher);
            await ImportData(sources, sourceStorage.Fields.Select(x => x.Name));
        }

        private async Task ImportData(IEnumerable<PersonSource> sources, IEnumerable<string> fieldNames)
        {
            IEnumerable<PhoneType> phoneTypes = await GetPhoneTypesAsync();

            PersonMapper mapper = new PersonMapper();

            using (SqlStorageContext<Person> storagePeople = new SqlStorageContext<Person>(_publisher))
            using (SqlStorageContext<Group> storageGroups = new SqlStorageContext<Group>(_publisher))
            {
                foreach (PersonSource source in sources)
                {
                    Person person = await storagePeople.Entities.FindByRescueGroupsIDAsync(source.ID);
                    if (person == null)
                    {
                        person = new Person()
                        {
                            RescueGroupsID = source.ID
                        };
                        storagePeople.Add(person);
                        _writer.WriteLine($"Added {source.ID} - {source.FirstName} {source.LastName}");
                    }
                    else
                    {
                        _writer.WriteLine($"Updated {source.ID} - {source.FirstName} {source.LastName}");
                    }

                    await AddOrUpdateGroupsAsync(storageGroups, source.GroupNames);

                    PersonMapperContext context = new PersonMapperContext(source, person, _publisher)
                    {
                        PhoneTypes = phoneTypes,
                        Groups = await storageGroups.GetAllAsync()
                    };

                    mapper.MapSourceToTarget(context, fieldNames);

                    await storagePeople.SaveChangesAsync();
                }
            }
        }
    }
}
