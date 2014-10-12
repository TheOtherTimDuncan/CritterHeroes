using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class OrderedListElementTests : BaseHtmlTest
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            OrderedListElement listElement = new OrderedListElement(GetHtmlHelper());
            Assert.AreEqual(@"<ol></ol>", listElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlyAddsInnerElement()
        {
            HtmlHelper htmlHelper = GetHtmlHelper();
            OrderedListElement listElement = new OrderedListElement(htmlHelper).AddElement(() =>
            {
                return new ListItemElement(htmlHelper).Text("text");
            });
            Assert.AreEqual(@"<ol><li>text</li></ol>", listElement.ToHtmlString());
        }
    }
}
