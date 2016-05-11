using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Shared.Commands;

namespace CritterHeroes.Web.Features.Shared.Models
{
    public class JsonError
    {
        public static JsonError FromCommandResult(CommandResult commandResult)
        {
            return new JsonError()
            {
                Errors=commandResult.Errors
            };
        }

        public static JsonError FromModelState(ModelStateDictionary modelState)
        {
            return new JsonError()
            {
                Errors = modelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToList()
            };
        }

        public IEnumerable<string> Errors
        {
            get;
            set;
        }
    }
}
