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
        public void DefaultsFormMethodToPostAndActionUrlToSelf()
        {
            new FormElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .StartWith("<form")
                .And
                .Contain("method=\"post\"", because: "post is the default")
                .And
                .Contain("action=\"/test/action\"", because: "Test is default controller and Action is default Action from RouteData defaults")
                .And
                .EndWith("></form>");
        }

        [TestMethod]
        public void CorrectlySetsFormMethodToPost()
        {
            new FormElement(GetHtmlHelper())
                .Method(FormMethod.Post)
                .ToHtmlString()
                .Should()
                .Contain("method=\"post\"", because: "post is assigned to Method");
        }

        [TestMethod]
        public void CorrectlySetsFormMethodToGet()
        {
            new FormElement(GetHtmlHelper())
                .Method(FormMethod.Get)
                .ToHtmlString()
                .Should()
                .Contain("method=\"get\"", because: "get is assigned to Method");
        }

        [TestMethod]
        public void CorrectlySetsFormActionToUrl()
        {
            new FormElement(GetHtmlHelper())
                .Action("http://google.com")
                .ToHtmlString()
                .Should()
                .StartWith("<form")
                .And
                .Contain("method=\"post\"", because: "post is the default")
                .And
                .Contain("action=\"http://google.com\"", because: "this is the url passed to action")
                .And
                .EndWith("></form>");
        }

        [TestMethod]
        public void CorrectlySetsFormActionToControllerAction()
        {
            new FormElement(GetHtmlHelper())
                .Action<TestController>(x => x.FormAction("returnUrl"))
                .ToHtmlString()
                .Should()
                .StartWith("<form")
                .And
                .Contain("method=\"post\"", because: "post is the default")
                .And
                .Contain("action=\"/Test/FormAction?returnUrl=returnUrl\"", because: "this is the url for the controller action")
                .And
                .EndWith("></form>");
        }

        [TestMethod]
        public void CorrectlyStartsAndEndsTagInUsingBlock()
        {
            HtmlHelper htmlHelper = GetHtmlHelper();

            using (FormElement formElement = new FormElement(htmlHelper).Begin())
            {
                htmlHelper
                    .ViewContext
                    .Writer
                    .ToString()
                    .Should()
                    .StartWith("<form")
                    .And
                    .EndWith(">");
            }

            htmlHelper
                .ViewContext
                .Writer
                .ToString()
                .Should()
                .EndWith("</form>");
        }
    }
}
