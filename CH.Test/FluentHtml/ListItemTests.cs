using System;
using System.Web.Mvc;
using CH.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.FluentHtml
{
    [TestClass]
    public class ListItemTests : BaseHtmlTest
    {
        [TestMethod]
        public void CreatesValidHtmlForElement()
        {
            new ListItemElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be("<li></li>");
        }

        [TestMethod]
        public void CorrectlySetsInnerText()
        {
            new ListItemElement(GetHtmlHelper())
                .Text("text")
                .ToHtmlString()
                .Should()
                .Be("<li>text</li>");
        }

        [TestMethod]
        public void HtmlEncodesInnerText()
        {
            new ListItemElement(GetHtmlHelper())
                .Text("<br/>")
                .ToHtmlString()
                .Should()
                .Be("<li>&lt;br/&gt;</li>");
        }

        [TestMethod]
        public void CorrectlyAddsInnerElement()
        {
            HtmlHelper htmlHelper = GetHtmlHelper();
            new ListItemElement(htmlHelper)
                .AddElement(() =>
                {
                    return new SpanElement(htmlHelper).Text("text");
                })
                .ToHtmlString()
                .Should()
                .Be("<li><span>text</span></li>");
        }
    }
}
