using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class SpeciesMappingTests : BaseTest
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new SpeciesSourceStorage(new RescueGroupsConfiguration(), null, null).ObjectType.Should().Be("animalSpecies");
        }

        [TestMethod]
        public void ConvertsJsonResultToModel()
        {
            SpeciesSource species1 = new SpeciesSource("1", "singular-1", "plural-1", "singular-young-1", "plural-young-1");
            SpeciesSource species2 = new SpeciesSource("2", "singular-2", "plural-2", "singular-young-2", "plural-young-2");

            JObject json = JObject.Parse(@"
{
    ""1"": {
        ""speciesID"": ""1"",
        ""speciesSingular"": ""singular-1"",
        ""speciesPlural"": ""plural-1"",
        ""speciesSingularYoung"": ""singular-young-1"",
        ""speciesPluralYoung"": ""plural-young-1""
    },
    ""2"": {
        ""speciesID"": ""2"",
        ""speciesSingular"": ""singular-2"",
        ""speciesPlural"": ""plural-2"",
        ""speciesSingularYoung"": ""singular-young-2"",
        ""speciesPluralYoung"": ""plural-young-2""
    }}
");

            IEnumerable<SpeciesSource> species = new SpeciesSourceStorage(new RescueGroupsConfiguration(), null, null).FromStorage(json.Properties());
            species.Should().HaveCount(2);

            SpeciesSource result1 = species.FirstOrDefault(x => x.Name == species1.Name);
            result1.Should().NotBeNull();
            result1.Singular.Should().Be(species1.Singular);
            result1.Plural.Should().Be(species1.Plural);
            result1.YoungSingular.Should().Be(species1.YoungSingular);
            result1.YoungPlural.Should().Be(species1.YoungPlural);

            SpeciesSource result2 = species.FirstOrDefault(x => x.Name == species2.Name);
            result2.Should().NotBeNull();
            result2.Singular.Should().Be(species2.Singular);
            result2.Plural.Should().Be(species2.Plural);
            result2.YoungSingular.Should().Be(result2.YoungSingular);
            result2.YoungPlural.Should().Be(result2.YoungPlural);
        }
    }
}
