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
            ListItemElement itemElement = new ListItemElement(GetHtmlHelper());
            Assert.AreEqual(@"<li></li>", itemElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsInnerText()
        {
            ListItemElement itemElement = new ListItemElement(GetHtmlHelper()).Text("text");
            Assert.AreEqual(@"<li>text</li>", itemElement.ToHtmlString());
        }

        [TestMethod]
        public void HtmlEncodesInnerText()
        {
            ListItemElement itemElement = new ListItemElement(GetHtmlHelper()).Text(@"<br/>");
            Assert.AreEqual(@"<li>&lt;br/&gt;</li>", itemElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlyAddsInnerElement()
        {
            HtmlHelper htmlHelper = GetHtmlHelper();
            ListItemElement itemElement = new ListItemElement(htmlHelper).AddElement(() =>
            {
                return new SpanElement(htmlHelper).Text("text");
            });
            Assert.AreEqual(@"<li><span>text</span></li>", itemElement.ToHtmlString());
        }
    }
}
