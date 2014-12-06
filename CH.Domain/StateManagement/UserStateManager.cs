using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using TOTD.Utility.StringHelpers;

namespace CH.Domain.StateManagement
{
    public class UserStateManager : StateManager<UserContext>
    {
        public UserStateManager(IOwinContext owinContext)
            : base(owinContext, "User")
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
