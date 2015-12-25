using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Data.Models;
using FluentAssertions;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.Misc;

namespace CH.Test
{
    public class BaseTest
    {
        public IEnumerable<Species> GetTestSupportedSpecies()
        {
            yield return new Species("1", "singular-1", "plural-1", null, null);
            yield return new Species("2", "singular-2", "plural-2", null, null);
        }

        public IEnumerable<SpeciesContext> GetTestSupportedSpeciesContext()
        {
            return GetTestSupportedSpecies().Select(x => SpeciesContext.FromSpecies(x));
        }

        public void AddTestSupportedCrittersToOrganization(Organization organization)
        {
            foreach (Species species in GetTestSupportedSpecies())
            {
                organization.AddSupportedCritter(species);
            }
        }

        public void AssertMethodsListIsNullOrEmpty(IEnumerable<MethodInfo> methods, string assertMessage)
        {
            string message = "Failing methods: " + string.Join("\n", methods.Select(x => x.DeclaringType.Name + "." + x.Name));
            Console.WriteLine(message);
            methods.Should().BeNullOrEmpty(assertMessage);
        }

        public T GetNonPublicPropertyValue<T>(object source, string propertyName)
        {
            PropertyInfo propertyInfo = source.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            object propertyValue = propertyInfo.GetValue(source, null);
            propertyValue.Should().NotBeNull(propertyName + " should exist in " + source.GetType().Name);

            if (propertyValue is IEnumerable)
            {
                return (T)propertyValue;
            }

            return (T)Convert.ChangeType(propertyValue, typeof(T));
        }
    }
}
