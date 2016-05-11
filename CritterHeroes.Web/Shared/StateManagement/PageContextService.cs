using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.StateManagement;
using Microsoft.Owin;

namespace CritterHeroes.Web.Shared.StateManagement
{
    public class PageContextService : IPageContextService
    {
        private const string _key = "CritterHeroes.Page";

        private IOwinContext _owinContext;

        public PageContextService(IOwinContext owinContext)
        {
            this._owinContext = owinContext;
        }

        public PageContext GetPageContext()
        {
            return _owinContext.Get<PageContext>(_key);
        }

        public void SavePageContext(PageContext pageContext)
        {
            _owinContext.Set(_key, pageContext);
        }
    }
}
