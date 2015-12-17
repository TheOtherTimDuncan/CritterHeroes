using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Mvc;

namespace CH.Test.ControllerTests.TestHelpers
{
    public class JsonCamelCaseResultTester : BaseJsonResultTester<JsonCamelCaseResultTester>
    {
        public JsonCamelCaseResultTester(JsonCamelCaseResult jsonResult)
           : base(jsonResult.Data)
        {
        }
    }
}
