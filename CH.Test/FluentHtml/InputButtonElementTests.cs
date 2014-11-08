using System;
using CH.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.FluentHtml
{
    [TestClass]
    public class InputButtonElementTests : BaseFormElementTests
    {
        [TestMethod]
        public void SuccessfullySetsInputType()
        {
            new InputButtonElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be(@"<input type=""button"" />");
        }
    }
}
