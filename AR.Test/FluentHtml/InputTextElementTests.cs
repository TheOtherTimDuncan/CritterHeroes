using System;
using AR.Website.Utility.FluentHtml.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class InputTextElementTests : BaseInputElementTests
    {
        [TestMethod]
        public void SuccessfullySetsInputType()
        {
            //HtmlHelper<TestModel> htmlHelper = GetHtmlHelper(GetViewData());
            //MvcHtmlString test = htmlHelper.TextBoxFor(x => x.Child.ChildStringProperty);
            InputTextElement inputElement = new InputTextElement(GetHtmlHelper());
            Assert.AreEqual(@"<input type=""text"" />", inputElement.ToHtmlString());
        }
    }
}
