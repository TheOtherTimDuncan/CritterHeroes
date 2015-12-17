using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Common.ActionResults;
using FluentAssertions;

namespace CH.Test.ControllerTests.TestHelpers
{
    public class RedirectToLocalResultTester
    {
        private RedirectToLocalResult _redirectResult;

        public RedirectToLocalResultTester(RedirectToLocalResult redirectResult)
        {
            this._redirectResult = redirectResult;
        }

        public RedirectToLocalResultTester HavingUrl(string url)
        {
            _redirectResult.Url.Should().Be(url);
            return this;
        }
    }
}
