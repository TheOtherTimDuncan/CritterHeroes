using System;
using System.Collections.Generic;
using CritterHeroes.Web.Shared.StateManagement;

namespace CritterHeroes.Web.Contracts.StateManagement
{
    public interface IPageContextService
    {
        PageContext GetPageContext();
        void SavePageContext(PageContext pageContext);
    }
}
