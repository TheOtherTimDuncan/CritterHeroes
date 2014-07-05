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
            OrderedListElement listElement = new OrderedListElement(GetViewContext());
            Assert.AreEqual(@"<ol></ol>", listElement.ToHtmlString());
        }

        [TestMethod]
        public void CorrectlyAddsInnerElement()
        {
            ViewContext viewContext = GetViewContext();
            OrderedListElement listElement = new OrderedListElement(viewContext).AddElement(() =>
            {
                return new ListItemElement(viewContext).Text("text");
            });
            Assert.AreEqual(@"<ol><li>text</li></ol>", listElement.ToHtmlString());
        }
    }
}
