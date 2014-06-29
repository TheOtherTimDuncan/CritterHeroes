using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class SpanElementTests
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            ViewContext viewContext = new ViewContext();
            SpanElement spanElement = new SpanElement(viewContext);
            Assert.AreEqual(@"<span></span>", spanElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsInnerText()
        {
            ViewContext viewContext = new ViewContext();
            SpanElement spanElement = new SpanElement(viewContext).Text("text");
            Assert.AreEqual(@"<span>text</span>", spanElement.ToHtmlString());
        }

        [TestMethod]
        public void HtmlEncodesInnerText()
        {
            ViewContext viewContext = new ViewContext();
            SpanElement spanElement = new SpanElement(viewContext).Text(@"<br/>");
            Assert.AreEqual(@"<span>&lt;br/&gt;</span>", spanElement.ToHtmlString());
        }
    }
}
