using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class OrderedListElementTests : BaseHtmlTest
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            new OrderedListElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be("<ol></ol>");
        }

        [TestMethod]
        public void CorrectlyAddsInnerElement()
        {
            HtmlHelper htmlHelper = GetHtmlHelper();
            new OrderedListElement(htmlHelper)
                .AddElement(() =>
                {
                    return new ListItemElement(htmlHelper).Text("text");
                })
                .ToHtmlString()
                .Should()
                .Be("<ol><li>text</li></ol>");
        }
    }
}
