using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CH.Dependency;
using FluentAssertions;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.Misc;

namespace CH.Test
{
    public class BaseTest
    {
        public T Using<T>()
        {
            return DependencyContainer.Using<T>();
        }

        public void AssertMethodsListIsNullOrEmpty(IEnumerable<MethodInfo> methods, string assertMessage)
        {
            string message = "Failing methods: " + string.Join("\n", methods.Select(x => x.DeclaringType.Name + "." + x.Name));
            Console.WriteLine(message);
            methods.Should().BeNullOrEmpty(assertMessage);
        }
    }
}
