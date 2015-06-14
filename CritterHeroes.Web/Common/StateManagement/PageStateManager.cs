using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.StateManagement;
using Microsoft.Owin;

namespace CritterHeroes.Web.Common.StateManagement
{
    public class PageStateManager : StateManager<PageContext>
    {
        public PageStateManager(IOwinContext owinContext, IStateSerializer serializer)
            : base(owinContext, serializer, key: "Page")
        {
        }

        protected override bool IsValid(PageContext context)
        {
            return true;
        }
    }
}