using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts;
using TOTD.Utility.StringHelpers;

namespace CH.Domain.StateManagement
{
    public class UserStateManager : StateManager<UserContext>
    {
        public UserStateManager(IHttpContext httpContext)
            : base(httpContext, "User")
        {
        }

        protected override bool IsValid(UserContext context)
        {
            if (context.DisplayName.IsNullOrEmpty())
            {
                return false;
            }

            return true;
        }
    }
}
