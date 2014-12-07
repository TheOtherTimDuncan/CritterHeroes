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
                where !t.IsInterface && !t.IsAbstract && t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == baseType)
                select t;

            VerifyTypes(handlerTypes);
        }

        [TestMethod]
        public void DependencyContainerIsBoundToAllCommandHandlers()
        {
            Type baseType = typeof(ICommandHandler<>).GetGenericTypeDefinition();

            IEnumerable<Type> handlerTypes =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where !t.IsInterface && !t.IsAbstract && t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == baseType)
                select t;

            VerifyTypes(handlerTypes);
        }

        public void VerifyTypes(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                try
                {
                    DependencyContainer.Get(type).Should().NotBeNull();
                }
                catch (ArgumentNullException ex)
                {
                    // Anything that depends on IHttpContext will throw an exception because the bound
                    // class requires HttpContext.Current to not be null. Lets ignore that exception but 
                    // rethrow any others
                    if (ex.ParamName != "HttpContext.Current")
                    {
                        throw;
                    }
                }
            }
        }
    }
}
