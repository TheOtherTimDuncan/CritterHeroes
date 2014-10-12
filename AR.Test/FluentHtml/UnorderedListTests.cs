using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class UnorderedListTests : BaseHtmlTest
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            UnorderedListElement listElement = new UnorderedListElement(GetHtmlHelper());
            Assert.AreEqual(@"<ul></ul>", listElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlyAddsInnerElement()
        {
            HtmlHelper htmlHelper = GetHtmlHelper();
            UnorderedListElement listElement = new UnorderedListElement(htmlHelper).AddElement(() =>
            {
                return new ListItemElement(htmlHelper).Text("text");
            });
            Assert.AreEqual(@"<ul><li>text</li></ul>", listElement.ToHtmlString());
        }
    }
}
