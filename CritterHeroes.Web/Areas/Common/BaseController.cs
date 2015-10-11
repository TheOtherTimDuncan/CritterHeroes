using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Models;
using CritterHeroes.Web.Common.ActionResults;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using Microsoft.AspNet.Identity;
using TOTD.Mvc;

namespace CritterHeroes.Web.Areas.Common
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

        protected  JsonCamelCaseResult JsonCamelCase( object data, HttpStatusCode statusCode, string contentType = null, Encoding contentEncoding = null)
        {
            return JsonCamelCase(data, (int)statusCode, contentType, contentEncoding);
        }

        protected  JsonCamelCaseResult JsonCamelCase( object data, int? statusCode = null, string contentType = null, Encoding contentEncoding = null)
        {
            return new JsonCamelCaseResult()
            {
                Data = data,
                StatusCode = statusCode
            };
        }

        protected void AddIdentityErrorsToModelState(ModelStateDictionary modelState, IdentityResult identityResult)
        {
            foreach (string error in identityResult.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected void AddCommandResultErrorsToModelState(ModelStateDictionary modelState, CommandResult commandResult)
        {
            foreach (string errorMessage in commandResult.Errors)
            {
                modelState.AddModelError("", errorMessage);
            }
        }

        protected JsonResult JsonCommandSuccess()
        {
            return Json(JsonCommandResult.Success());
        }

        protected JsonResult JsonCommandError(ModelStateDictionary modelState)
        {
            IEnumerable<string> errors =
                from v in modelState.Values
                from e in v.Errors
                select e.ErrorMessage;
            return Json(JsonCommandResult.Error(string.Join(", ", errors)));
        }
    }
}