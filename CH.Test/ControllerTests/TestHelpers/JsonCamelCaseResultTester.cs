using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using TOTD.Mvc;

namespace CH.Test.ControllerTests.TestHelpers
{
    public class JsonCamelCaseResultTester : BaseJsonResultTester<JsonCamelCaseResultTester>
    {
        private JsonCamelCaseResult _jsonResult;

        public JsonCamelCaseResultTester(JsonCamelCaseResult jsonResult)
           : base(jsonResult.Data)
        {
            this._jsonResult = jsonResult;
        }

        public JsonCamelCaseResultTester HavingStatusCode(HttpStatusCode statusCode)
        {
            _jsonResult.StatusCode.Should().Be((int)statusCode);
            return this;
        }
    }
}
