using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentAssertions;

namespace CH.Test.ControllerTests.TestHelpers
{
    public class ViewResultTester<ViewResultType> where ViewResultType : ViewResultBase
    {
        private ViewResultBase _viewResult;

        public ViewResultTester(ViewResultType viewResult)
        {
            this._viewResult = viewResult;
        }

        public ViewResultTester<ViewResultType> HavingModel(object model)
        {
            _viewResult.Model.Should().Be(model);
            return this;
        }

        public ViewResultTester<ViewResultType> HavingModel<ModelType>()
        {
            _viewResult.Model.Should().BeOfType<ModelType>();
            return this;
        }

        public ViewResultTester<ViewResultType> HavingModel<ModelType>(Action<ModelType> action) where ModelType : class
        {
            ModelType model = _viewResult.Model as ModelType;
            model.Should().NotBeNull("expected " + typeof(ModelType).Name + " to be returned");
            action(model);
            return this;
        }
    }
}
