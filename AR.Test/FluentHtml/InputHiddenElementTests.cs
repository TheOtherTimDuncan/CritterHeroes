using System;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class InputHiddenElementTests : BaseFormElementTests
    {
        [TestMethod]
        public void SuccessfullySetsInputType()
        {
            new InputHiddenElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be(@"<input type=""hidden"" />");
        }
    }
}
