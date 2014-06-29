using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class LinkElementTests
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            ViewContext viewContext = new ViewContext();
            LinkElement linkElement = new LinkElement(viewContext);
            Assert.AreEqual(@"<a></a>", linkElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsInnerText()
        {
            ViewContext viewContext = new ViewContext();
            LinkElement linkElement = new LinkElement(viewContext).Text("text");
            Assert.AreEqual(@"<a>text</a>", linkElement.ToHtmlString());
        }

        [TestMethod]
        public void HtmlEncodesInnerText()
        {
            ViewContext viewContext = new ViewContext();
            LinkElement linkElement = new LinkElement(viewContext).Text(@"<br/>");
            Assert.AreEqual(@"<a>&lt;br/&gt;</a>", linkElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsJavascriptLink()
        {
            ViewContext viewContext = new ViewContext();
            LinkElement linkElement = new LinkElement(viewContext).AsJavascriptLink();
            Assert.AreEqual(@"<a href=""#""></a>", linkElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsUrl()
        {
            ViewContext viewContext = new ViewContext();
            LinkElement linkElement = new LinkElement(viewContext).Url("http://www.google.com");
            Assert.AreEqual(@"<a href=""http://www.google.com""></a>", linkElement.ToHtmlString());
        }
    }
}
