﻿using System;
using AR.Website.Utility.FluentHtml.Elements;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class InputPasswordElementTests : BaseFormElementTests
    {
        [TestMethod]
        public void SuccessfullySetsInputType()
        {
            new InputPasswordElement(GetHtmlHelper())
                .ToHtmlString()
                .Should()
                .Be(@"<input type=""password"" />");
        }
    }
}