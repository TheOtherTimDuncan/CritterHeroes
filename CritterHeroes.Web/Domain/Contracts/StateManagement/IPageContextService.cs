using System;
using System.Collections.Generic;
using CritterHeroes.Web.Shared.StateManagement;

namespace CritterHeroes.Web.Domain.Contracts.StateManagement
{
    public interface IPageContextService
    {
        PageContext GetPageContext();
        void SavePageContext(PageContext pageContext);
    }
}
