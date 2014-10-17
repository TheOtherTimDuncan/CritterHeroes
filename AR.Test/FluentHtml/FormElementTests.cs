using System;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class FormElementTests : BaseElementTests
    {
        [TestMethod]
        public void DefaultsFormMethodToPost()
        {
            new FormElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be("<form method=\"post\"></form>");
        }

        [TestMethod]
        public void CorrectlySetsFormMethodToPost()
        {
            new FormElement(GetHtmlHelper())
                .Method(FormMethod.Post)
                .ToHtmlString()
                .Should()
                .Be("<form method=\"post\"></form>");
        }

        [TestMethod]
        public void CorrectlySetsFormMethodToGet()
        {
            new FormElement(GetHtmlHelper())
                .Method(FormMethod.Get)
                .ToHtmlString()
                .Should()
                .Be("<form method=\"get\"></form>");
        }

        [TestMethod]
        public void CorrectlyStartsAndEndsTagInUsingBlock()
        {
            HtmlHelper htmlHelper = GetHtmlHelper();
            using (FormElement formElement = new FormElement(htmlHelper).Begin())
            {
                htmlHelper.ViewContext.Writer.ToString().Should().Be("<form method=\"post\">");
            }
            htmlHelper.ViewContext.Writer.ToString().Should().Be("<form method=\"post\"></form>");
        }
    }
}
