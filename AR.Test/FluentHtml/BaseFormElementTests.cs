using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class BaseFormElementTests : BaseElementTests
    {
        private ViewDataDictionary<TestModel> GetViewData(TestModel model)
        {
            ViewDataDictionary<TestModel> viewData = new ViewDataDictionary<TestModel>(model)
            {
                {"StringProperty", model.StringProperty},
                {"IntegerProperty", model.IntegerProperty}
            };
            viewData.Model = model;

            return viewData;
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
            new InputTextElement(htmlHelper)
                .For<TestModel>(x => x.StringProperty)
                .ToHtmlString().Should()
                .StartWith("<input")
                .And
                .EndWith("/>")
                .And
                .Contain("id=\"StringProperty\"")
                .And
                .Contain("name=\"StringProperty\"")
                .And
                .Contain("type=\"text\"")
                .And
                .Contain("value=\"StringValue\"");
        }

        [TestMethod]
        public void CorrectlySetsAutoFocusAttribute()
        {
            TestModel model = new TestModel
            {
                StringProperty = "StringValue"
            };
            ViewDataDictionary<TestModel> viewData = GetViewData(model);
            new InputTextElement(GetHtmlHelper(viewData))
                .AutoFocus()
                .ToHtmlString()
                .Should()
                .StartWith("<input")
                .And
                .EndWith("/>")
                .And
                .Contain("autofocus=\"autofocus\"");
        }

        [TestMethod]
        public void CorrectlySetsRequiredAttribute()
        {
            TestModel model = new TestModel
            {
                StringProperty = "StringValue"
            };
            ViewDataDictionary<TestModel> viewData = GetViewData(model);
            new InputTextElement(GetHtmlHelper(viewData))
                .Required()
                .ToHtmlString()
                .Should()
                .StartWith("<input")
                .And
                .EndWith("/>")
                .And
                .Contain("required=\"required\"");
        }

        public class TestModel
        {
            public int IntegerProperty
            {
                get;
                set;
            }

            public string StringProperty
            {
                get;
                set;
            }

            public ChildModel Child
            {
                get;
                set;
            }
        }

        public class ChildModel
        {
            public string ChildStringProperty
            {
                get;
                set;
            }
        }
    }
}
