using System;
using System.Collections.Generic;
using CritterHeroes.Web.Common.StateManagement;

namespace CritterHeroes.Web.Contracts.StateManagement
{
    public interface IPageContextService
    {
        PageContext GetPageContext();
        void SavePageContext(PageContext pageContext);
    }
}
