using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using SimpleInjector.Diagnostics;

namespace CH.Test
{
    [TestClass]
    public class DependencyTests
    {
        [TestMethod]
        public void DependencyContainerIsValid()
        {
            using (Container container = DIConfig.ConfigureDependencyContainer())
            {
                container.Verify();

                IEnumerable<DiagnosticResult> results = Analyzer.Analyze(container).Where(x =>
                {
                    if (x.DiagnosticType == DiagnosticType.DisposableTransientComponent && typeof(Controller).IsAssignableFrom(x.ServiceType))
                    {
                        // Ignore Transient lifestyle for IDisposable warning for controllers
                        return false;
                    }

                    return true;
                });
                results.Should().BeNullOrEmpty(string.Join(Environment.NewLine, results.Select(x => x.Description)));
            }
        }
    }
}
