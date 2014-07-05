using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class ListItemTests : BaseHtmlTest
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            ListItemElement itemElement = new ListItemElement(GetViewContext());
            Assert.AreEqual(@"<li></li>", itemElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsInnerText()
        {
            ListItemElement itemElement = new ListItemElement(GetViewContext()).Text("text");
            Assert.AreEqual(@"<li>text</li>", itemElement.ToHtmlString());
        }

        [TestMethod]
        public void HtmlEncodesInnerText()
        {
            ListItemElement itemElement = new ListItemElement(GetViewContext()).Text(@"<br/>");
            Assert.AreEqual(@"<li>&lt;br/&gt;</li>", itemElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlyAddsInnerElement()
        {
            ViewContext viewContext = GetViewContext();
            ListItemElement itemElement = new ListItemElement(viewContext).AddElement(() =>
            {
                return new SpanElement(viewContext).Text("text");
            });
            Assert.AreEqual(@"<li><span>text</span></li>", itemElement.ToHtmlString());
        }
    }
}
