using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class InputPasswordElementTests : BaseFormElementTests
    {
        [TestMethod]
        public void SuccessfullySetsInputType()
        {
            new InputPasswordElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be(@"<input type=""password"" />");
        }

        [TestMethod]
        public void SetsElementIDNameAndValueFromModelExpression()
        {
            TestModel model = new TestModel
            {
                StringProperty = "StringValue"
            };

            ViewDataDictionary<TestModel> viewData = GetViewData(model);
            HtmlHelper<TestModel> htmlHelper = GetHtmlHelper(viewData);
            htmlHelper
                .FluentHtml()
                .InputPassword()
                .For(x => x.StringProperty)
                .ToHtmlString()
                .Should()
                .StartWith("<input")
                .And
                .EndWith("/>")
                .And
                .Contain("id=\"StringProperty\"")
                .And
                .Contain("name=\"StringProperty\"")
                .And
                .Contain("type=\"password\"")
                .And
                .Contain("value=\"StringValue\"");
        }
    }
}
