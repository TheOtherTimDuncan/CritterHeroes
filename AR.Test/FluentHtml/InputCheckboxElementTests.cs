using System;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
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
