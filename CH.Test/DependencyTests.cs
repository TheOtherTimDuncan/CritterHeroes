using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;
using CH.Website;
using CH.Website.Dependency;
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
            Type[] queryHandlerTypes = new Type[] { typeof(IQueryHandler<,>), typeof(IAsyncQueryHandler<,>) };

            IEnumerable<Type> handlerTypes =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where
                    !t.IsInterface
                    && !t.IsAbstract
                    && t.GetInterfaces().Any(x => x.IsGenericType && (queryHandlerTypes.Any(h => h == x.GetGenericTypeDefinition())))
                select t;

            VerifyTypes(handlerTypes);
        }

        [TestMethod]
        public void DependencyContainerIsBoundToAllCommandHandlers()
        {
            Type[] commandHandlerTypes = new Type[] { typeof(IAsyncCommandHandler<>), typeof(ICommandHandler<>) };

            IEnumerable<Type> handlerTypes =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where
                    !t.IsInterface
                    && !t.IsAbstract
                    && t.GetInterfaces().Any(x => x.IsGenericType && (commandHandlerTypes.Any(h => h == x.GetGenericTypeDefinition())))
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
