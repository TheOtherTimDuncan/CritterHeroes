using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Features.Common.ActionExtensions;

namespace CritterHeroes.Web.Common.ActionResults
{
    public class RedirectToLocalResult : ActionResult
    {
        private string _url;

        public RedirectToLocalResult(string url)
        {
            this._url = url;
        }

        public string Url
        {
            get
            {
                return _url;
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            UrlHelper urlHelper = new UrlHelper(context.RequestContext);
            string redirectUrl = urlHelper.Local(_url);
            new RedirectResult(redirectUrl).ExecuteResult(context);
        }
    }
}
