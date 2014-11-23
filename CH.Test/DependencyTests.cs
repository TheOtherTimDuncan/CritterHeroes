using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Dependency;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;
using CH.Website;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test
{
    [TestClass]
    public class DependencyTests
    {
        [TestMethod]
        public void DependencyContainerIsBoundToAllQueryHandlers()
        {
            Type baseType = typeof(IQueryHandler<,>).GetGenericTypeDefinition();

            IEnumerable<Type> handlerTypes =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where !t.IsInterface && t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == baseType)
                select t;

            foreach (Type type in handlerTypes)
            {
                DependencyContainer.Get(type).Should().NotBeNull();
            }
        }

        [TestMethod]
        public void DependencyContainerIsBoundToAllCommandHandlers()
        {
            Type baseType = typeof(ICommandHandler<>).GetGenericTypeDefinition();

            IEnumerable<Type> handlerTypes =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where !t.IsInterface && t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == baseType)
                select t;

            foreach (Type type in handlerTypes)
            {
                DependencyContainer.Get(type).Should().NotBeNull();
            }
        }
    }
}
