using System;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class InputTextElementTests : BaseInputElementTests
    {
        [TestMethod]
        public void SuccessfullySetsInputType()
        {
            new InputTextElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be(@"<input type=""text"" />");
        }
    }
}
