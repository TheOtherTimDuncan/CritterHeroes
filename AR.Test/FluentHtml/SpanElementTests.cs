using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class SpanElementTests : BaseHtmlTest
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            new SpanElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be("<span></span>");
        }

        [TestMethod]
        public void CorrectlySetsInnerText()
        {
            new SpanElement(GetHtmlHelper())
                .Text("text")
                .ToHtmlString()
                .Should()
                .Be("<span>text</span>");
        }

        [TestMethod]
        public void HtmlEncodesInnerText()
        {
            new SpanElement(GetHtmlHelper())
                .Text(@"<br/>")
                .ToHtmlString()
                .Should()
                .Be("<span>&lt;br/&gt;</span>");
        }
    }
}
