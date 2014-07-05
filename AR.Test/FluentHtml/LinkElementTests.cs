using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class LinkElementTests : BaseHtmlTest
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            LinkElement linkElement = new LinkElement(GetViewContext());
            Assert.AreEqual(@"<a></a>", linkElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsInnerText()
        {
            LinkElement linkElement = new LinkElement(GetViewContext()).Text("text");
            Assert.AreEqual(@"<a>text</a>", linkElement.ToHtmlString());
        }

        [TestMethod]
        public void HtmlEncodesInnerText()
        {
            LinkElement linkElement = new LinkElement(GetViewContext()).Text(@"<br/>");
            Assert.AreEqual(@"<a>&lt;br/&gt;</a>", linkElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsJavascriptLink()
        {
            LinkElement linkElement = new LinkElement(GetViewContext()).AsJavascriptLink();
            Assert.AreEqual(@"<a href=""#""></a>", linkElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlySetsUrl()
        {
            LinkElement linkElement = new LinkElement(GetViewContext()).Url("http://www.google.com");
            Assert.AreEqual(@"<a href=""http://www.google.com""></a>", linkElement.ToHtmlString());
        }
    }
}
