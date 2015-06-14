using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.StateManagement;
using TOTD.Utility.Misc;

namespace CritterHeroes.Web.Common.ActionResults
{
    public class RedirectToPreviousResult : ActionResult
    {
        private IPageContextService _pageContextService;

        public RedirectToPreviousResult()
        {
            this._pageContextService = DependencyResolver.Current.GetService<IPageContextService>();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string url = _pageContextService.GetPageContext().IfNotNull(x => x.PreviousPath);
            new RedirectToLocalResult(url).ExecuteResult(context);
        }
    }
}