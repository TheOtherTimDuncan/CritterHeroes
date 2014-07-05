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
            UnorderedListElement listElement = new UnorderedListElement(GetViewContext());
            Assert.AreEqual(@"<ul></ul>", listElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlyAddsInnerElement()
        {
            ViewContext viewContext = GetViewContext();
            UnorderedListElement listElement = new UnorderedListElement(viewContext).AddElement(() =>
            {
                return new ListItemElement(viewContext).Text("text");
            });
            Assert.AreEqual(@"<ul><li>text</li></ul>", listElement.ToHtmlString());
        }
    }
}
