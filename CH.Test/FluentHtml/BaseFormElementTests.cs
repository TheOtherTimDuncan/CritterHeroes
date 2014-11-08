using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CH.Website.Utility.FluentHtml;
using CH.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.FluentHtml
{
    [TestClass]
    public class BaseFormElementTests : BaseElementTests
    {
        public ViewDataDictionary<TestModel> GetViewData(TestModel model)
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

            [Display(Name = "Display Label")]
            public string DisplayProperty
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
