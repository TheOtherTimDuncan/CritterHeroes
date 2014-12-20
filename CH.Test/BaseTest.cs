using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CH.Domain.Models.Data;
using FluentAssertions;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.Misc;

namespace CH.Test
{
    public class BaseTest
    {
        public IEnumerable<Species> GetTestSupportedSpecies()
        {
            return new Species[] 
            { 
                new Species("1","singular-1","plural-2", null, null),
                new Species("2","singular-2","plural-2", null, null) 
            };
        }

        public void AssertMethodsListIsNullOrEmpty(IEnumerable<MethodInfo> methods, string assertMessage)
        {
            string message = "Failing methods: " + string.Join("\n", methods.Select(x => x.DeclaringType.Name + "." + x.Name));
            Console.WriteLine(message);
            methods.Should().BeNullOrEmpty(assertMessage);
        }
    }
}
