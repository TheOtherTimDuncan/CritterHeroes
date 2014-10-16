using System;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class InputSubmitElementTests : BaseFormElementTests
    {
        [TestMethod]
        public void SuccessfullySetsInputType()
        {
            new InputSubmitElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be(@"<input type=""submit"" />");
        }
    }
}
