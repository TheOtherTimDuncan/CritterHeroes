using System;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
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
