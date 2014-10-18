using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class LabelElementTests : BaseFormElementTests
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            new LabelElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be("<label></label>");
        }
        
        [TestMethod]
        public void SuccessfullySetsInnerText()
        {
            new LabelElement(GetHtmlHelper())
                .Text("test")
                .ToHtmlString()
                .Should()
                .Be("<label>test</label>");
        }

        [TestMethod]
        public void SuccessfullySetsTarget()
        {
            new LabelElement(GetHtmlHelper())
                .For("test")
                .ToHtmlString()
                .Should()
                .Be("<label for=\"test\"></label>");
        }

        [TestMethod]
        public void SuccessfullySetsTargetFromModelProperty()
        {
            TestModel model = new TestModel
            {
                StringProperty = "StringValue"
            };

            ViewDataDictionary<TestModel> viewData = GetViewData(model);
            HtmlHelper<TestModel> htmlHelper = GetHtmlHelper(viewData);
            htmlHelper
                .FluentHtml()
                .Label()
                .For(x => x.StringProperty)
                .ToHtmlString()
                .Should()
                .Be("<label for=\"StringProperty\">StringProperty</label>");
        }

        [TestMethod]
        public void SuccessfullySetsLabelTextFromModelPropertyWithDisplayAttribute()
        {
            TestModel model = new TestModel
            {
                DisplayProperty = "StringValue"
            };

            ViewDataDictionary<TestModel> viewData = GetViewData(model);
            HtmlHelper<TestModel> htmlHelper = GetHtmlHelper(viewData);
            htmlHelper
                .FluentHtml()
                .Label()
                .For(x => x.DisplayProperty)
                .ToHtmlString()
                .Should()
                .Be("<label for=\"DisplayProperty\">Display Label</label>");
        }
    }
}
