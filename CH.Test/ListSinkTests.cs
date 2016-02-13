using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CritterHeroes.Web.Common;
using Serilog;

namespace CH.Test
{
    [TestClass]
    public class ListSinkTests
    {
        [TestMethod]
        public void AddsLogEventToList()
        {
            List<string> messages = new List<string>();

            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .List(messages)
                .MinimumLevel.Debug()
                .CreateLogger();

            Log.Debug("This is a {Test}", "test");

            messages.Should().HaveCount(1);
            messages.Single().Should().Be("This is a \"test\"");
        }
    }
}
