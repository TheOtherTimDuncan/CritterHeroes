using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class UnorderedListTests : BaseHtmlTest
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            new UnorderedListElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be("<ul></ul>");
        }

        [TestMethod]
        public void CorrectlyAddsInnerElement()
        {
            HtmlHelper htmlHelper = GetHtmlHelper();
            new UnorderedListElement(htmlHelper)
                .AddElement(() =>
                {
                    return new ListItemElement(htmlHelper).Text("text");
                })
                .ToHtmlString()
                .Should()
                .Be("<ul><li>text</li></ul>");
        }
    }
}
