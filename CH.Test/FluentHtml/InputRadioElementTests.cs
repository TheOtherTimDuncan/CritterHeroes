using System;
using CH.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.FluentHtml
{
    [TestClass]
    public class InputRadioElementTests : BaseFormElementTests
    {
        [TestMethod]
        public void SuccessfullySetsInputType()
        {
            new InputRadioElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be(@"<input type=""radio"" />");
        }
    }
}
