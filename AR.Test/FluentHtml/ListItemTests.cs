using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class ListItemTests
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            ViewContext viewContext = new ViewContext();
            ListItemElement itemElement = new ListItemElement(viewContext);
            Assert.AreEqual(@"<li></li>", itemElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsInnerText()
        {
            ViewContext viewContext = new ViewContext();
            ListItemElement itemElement = new ListItemElement(viewContext).Text("text");
            Assert.AreEqual(@"<li>text</li>", itemElement.ToHtmlString());
        }

        [TestMethod]
        public void HtmlEncodesInnerText()
        {
            ViewContext viewContext = new ViewContext();
            ListItemElement itemElement = new ListItemElement(viewContext).Text(@"<br/>");
            Assert.AreEqual(@"<li>&lt;br/&gt;</li>", itemElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlyAddsInnerElement()
        {
            ViewContext viewContext = new ViewContext();
            ListItemElement itemElement = new ListItemElement(viewContext).AddElement(() =>
            {
                return new SpanElement(viewContext).Text("text");
            });
            Assert.AreEqual(@"<li><span>text</span></li>", itemElement.ToHtmlString());
        }
    }
}
