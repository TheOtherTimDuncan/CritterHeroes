using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using Microsoft.Owin;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Shared.StateManagement
{
    public class UserStateManager : StateManager<UserContext>
    {
        public UserStateManager(IOwinContext owinContext, IStateSerializer serializer)
            : base(owinContext, serializer, "User")
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
