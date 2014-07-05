using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class SpanElementTests : BaseHtmlTest
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            SpanElement spanElement = new SpanElement(GetViewContext());
            Assert.AreEqual(@"<span></span>", spanElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsInnerText()
        {
            SpanElement spanElement = new SpanElement(GetViewContext()).Text("text");
            Assert.AreEqual(@"<span>text</span>", spanElement.ToHtmlString());
        }

        [TestMethod]
        public void HtmlEncodesInnerText()
        {
            SpanElement spanElement = new SpanElement(GetViewContext()).Text(@"<br/>");
            Assert.AreEqual(@"<span>&lt;br/&gt;</span>", spanElement.ToHtmlString());
        }
    }
}
