using System;
using System.Collections.Generic;

namespace AR.Website.Utility.FluentHtml.Contracts
{
    public interface IUrlContext
    {
        bool Matches(IUrlContext urlContext);
    }
}
