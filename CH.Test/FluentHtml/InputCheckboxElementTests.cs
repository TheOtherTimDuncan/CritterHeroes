using System;
using CH.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.FluentHtml
{
    [TestClass]
    public class InputCheckboxElementTests : BaseFormElementTests
    {
        [TestMethod]
        public void SuccessfullySetsInputType()
        {
            new InputCheckboxElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be(@"<input type=""checkbox"" />");
        }
    }
}
