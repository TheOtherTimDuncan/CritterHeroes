using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts.Commands;
using Microsoft.AspNet.Identity;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Services.Commands
{
    public abstract class BaseCommandResult<T> : ICommandResult where T : BaseCommandResult<T>
    {
        protected BaseCommandResult()
        {
            this.Errors = new Dictionary<string, List<string>>();
        }

        private static T Failed()
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            result.Succeeded = false;
            return result;
        }

        public static T Success()
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            result.Succeeded = true;
            return result;
        }

        public static T Failed(string errorKey, string errorMessage)
        {
            return Failed().AddError(errorKey, errorMessage);
        }

        public static T Failed(string errorKey, IEnumerable<string> errorMessages)
        {
            return Failed().AddErrors(errorKey, errorMessages);
        }

        public static T FromIdentityResult(IdentityResult identityResult, string errorKey)
        {
            if (identityResult.Succeeded)
            {
                return Success();
            }
            else
            {
                return Failed().AddIdentityResultErrors(errorKey, identityResult);
            }
        }

        public bool Succeeded
        {
            get;
            private set;
        }

        public IDictionary<string, List<string>> Errors
        {
            get;
            private set;
        }

        public T AddError(string key, string error)
        {
            ThrowIf.Argument.IsNull(key, "key");
            ThrowIf.Argument.IsNullOrEmpty(error, "error");

            List<string> errors;
            if (!Errors.TryGetValue(key, out errors))
            {
                errors = new List<string>();
                Errors[key] = errors;
            }

            errors.Add(error);

            return (T)this;
        }

        public T AddIdentityResultErrors(string key, IdentityResult identityResult)
        {
            ThrowIf.Argument.IsNull(identityResult, "identityResult");
            return AddErrors(key, identityResult.Errors);
        }

        public T AddErrors(string key, IEnumerable<string> errors)
        {
            ThrowIf.Argument.IsNull(key, "key");
            ThrowIf.Argument.IsNull(errors, "errors");

            foreach (string error in errors)
            {
                AddError(key, error);
            }

            return (T)this;
        }
    }
}
