using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Commands
{
    public class CommandResult
    {
        private CommandResult(bool succeeded)
        {
            this.Errors = new Dictionary<string, string>();
            this.Succeeded = succeeded;
        }

        public static CommandResult Success()
        {
            return new CommandResult(true);
        }

        public static CommandResult Failed(string errorKey, string errorMessage)
        {
            return new CommandResult(false).AddError(errorKey, errorMessage);
        }

        public static CommandResult FromIdentityResult(IdentityResult identityResult, string errorKey)
        {
            if (identityResult.Succeeded)
            {
                return Success();
            }
            else
            {
                return new CommandResult(false).AddIdentityResultErrors(errorKey, identityResult);
            }
        }

        public bool Succeeded
        {
            get;
            private set;
        }

        public IDictionary<string, string> Errors
        {
            get;
            private set;
        }

        public CommandResult AddError(string key, string error)
        {
            ThrowIf.Argument.IsNull(key, "key");
            ThrowIf.Argument.IsNullOrEmpty(error, "error");
            Errors[key] = error;
            return this;
        }

        public CommandResult AddIdentityResultErrors(string key, IdentityResult identityResult)
        {
            ThrowIf.Argument.IsNull(key, "key");
            ThrowIf.Argument.IsNull(identityResult, "identityResult");

            foreach (string error in identityResult.Errors)
            {
                AddError(key, error);
            }

            return this;
        }
    }
}
