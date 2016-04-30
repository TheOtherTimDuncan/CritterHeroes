using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using CritterHeroes.Web.Common.ActionResults;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using TOTD.Mvc;

namespace CritterHeroes.Web.Features.Common
{
    public class BaseController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public BaseController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        protected IQueryDispatcher QueryDispatcher
        {
            get
            {
                return _queryDispatcher;
            }
        }

        protected ICommandDispatcher CommandDispatcher
        {
            get
            {
                return _commandDispatcher;
            }
        }

        protected RedirectToPreviousResult RedirectToPrevious()
        {
            return new RedirectToPreviousResult();
        }

        protected RedirectToLocalResult RedirectToLocal(string redirectUrl)
        {
            return new RedirectToLocalResult(redirectUrl);
        }

        protected void AddCommandResultErrorsToModelState(ModelStateDictionary modelState, CommandResult commandResult)
        {
            foreach (string errorMessage in commandResult.Errors)
            {
                modelState.AddModelError("", errorMessage);
            }
        }

        protected HttpStatusCodeResult StatusCode(HttpStatusCode statusCode)
        {
            return new HttpStatusCodeResult(statusCode);
        }

        protected JsonCamelCaseResult JsonCamelCase(object data, HttpStatusCode statusCode, string contentType = null, Encoding contentEncoding = null)
        {
            return JsonCamelCase(data, (int)statusCode, contentType, contentEncoding);
        }

        protected JsonCamelCaseResult JsonCamelCase(object data, int? statusCode = null, string contentType = null, Encoding contentEncoding = null)
        {
            return new JsonCamelCaseResult()
            {
                Data = data,
                StatusCode = statusCode
            };
        }
    }
}
