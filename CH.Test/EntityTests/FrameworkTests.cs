using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Contexts;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.EntityTests
{
    [TestClass]
    public class FrameworkTests : BaseTest
    {
        [TestMethod]
        public void AllEntityClassesShouldHaveDefaultEmptyConstructor()
        {
            var modelTypes =
                from t in typeof(ISqlStorageContext<>).Assembly.GetExportedTypes()
                where t.Namespace.StartsWith("CritterHeroes.Web.Data.Models") && t.IsClass && t.IsPublic && !t.IsAbstract
                select new
                {
                    ModelType = t,
                    ModelConstructor = t.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null)
                };

            modelTypes.Should().NotBeNullOrEmpty("doesn't help to verify entity classes if we can't find their models");

            var invalidTypes =
                from t in modelTypes
                where (t.ModelConstructor == null || t.ModelConstructor.IsPrivate)
                select t.ModelType;

            invalidTypes.Should().BeNullOrEmpty("Entity Framework models need a public or protected parameterless constructor: " + string.Join(",", invalidTypes.Select(x => x.Name)));
        }
    }
}
