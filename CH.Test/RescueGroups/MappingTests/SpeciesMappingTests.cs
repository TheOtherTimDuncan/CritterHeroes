using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models.Data;
using CH.RescueGroups;
using CH.RescueGroups.Mappings;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CH.Test.RescueGroups.MappingTests
{
    [TestClass]
    public class SpeciesMappingTests
    {
        public IRescueGroupsMapping<Species> Mapping
        {
            get
            {
                return RescueGroupsMappingFactory.GetMapping<Species>();
            }
        }

        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            Mapping.ObjectType.Should().Be("animalSpecies");
        }

        [TestMethod]
        public void ConvertsJsonResultToModel()
        {
            Species species1 = new Species("1", "singular-1", "plural-1", "singular-young-1", "plural-young=1");
            Species species2 = new Species("2", "singular-2", "plural-2", "singular-young-2", "plural-young=2");

            JProperty element1 = new JProperty("1", new JObject(
                new JProperty("speciesSingular", species1.Singular),
                new JProperty("speciesPlural", species1.Plural),
                new JProperty("speciesSingularYoung", species1.YoungSingular),
                new JProperty("speciesPluralYoung", species1.YoungPlural)
                )
            );
            JProperty element2 = new JProperty("2", new JObject(
                new JProperty("speciesSingular", species2.Singular),
                new JProperty("speciesPlural", species2.Plural),
                new JProperty("speciesSingularYoung", species2.YoungSingular),
                new JProperty("speciesPluralYoung", species2.YoungPlural)
                )
            );

            JObject data = new JObject();
            data.Add(element1);
            data.Add(element2);

            IEnumerable<Species> species = Mapping.ToModel(data.Properties());
            species.Should().HaveCount(2);

            Species result1 = species.FirstOrDefault(x => x.Name == species1.Name);
            result1.Should().NotBeNull();
            result1.Singular.Should().Be(species1.Singular);
            result1.Plural.Should().Be(species1.Plural);
            result1.YoungSingular.Should().Be(species1.YoungSingular);
            result1.YoungPlural.Should().Be(species1.YoungPlural);

            Species result2 = species.FirstOrDefault(x => x.Name == species2.Name);
            result2.Should().NotBeNull();
            result2.Singular.Should().Be(species2.Singular);
            result2.Plural.Should().Be(species2.Plural);
            result2.YoungSingular.Should().Be(result2.YoungSingular);
            result2.YoungPlural.Should().Be(result2.YoungPlural);
        }

        [TestMethod]
        public async Task TestGetSpecies()
        {
            RescueGroupsStorage storage = new RescueGroupsStorage();
            (await storage.GetAllAsync<Species>()).ToList().Should().NotBeEmpty();
        }

    }
}
