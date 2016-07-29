using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentAssertions;

namespace CH.Test.ControllerTests.TestHelpers
{
    public class ViewResultTester<TViewResult> where TViewResult : ViewResultBase
    {
        private ViewResultBase _viewResult;

        public ViewResultTester(TViewResult viewResult)
        {
            this._viewResult = viewResult;
        }

        public ViewResultTester<TViewResult> HavingModel(object model)
        {
            _viewResult.Model.Should().Be(model);
            return this;
        }

        public ViewResultTester<TViewResult> HavingModel<TModel>()
        {
            _viewResult.Model.Should().BeOfType<TModel>();
            return this;
        }

        public ViewResultTester<TViewResult> HavingModel<TModel>(Action<TModel> action) where TModel : class
        {
            TModel model = _viewResult.Model as TModel;
            model.Should().NotBeNull("expected " + typeof(TModel).Name + " to be returned");
            action(model);
            return this;
        }

        public ViewResultTester<TViewResult> HavingDefaultView()
        {
            _viewResult.ViewName.Should().BeNullOrEmpty();
            return this;
        }

        public ViewResultTester<TViewResult> HavingView(string viewName)
        {
            _viewResult.ViewName.Should().Be(viewName);
            return this;
        }
    }
}
