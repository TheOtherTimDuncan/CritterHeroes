using System;
using System.Collections.Generic;

namespace CH.Website.Utility.FluentHtml.Contracts
{
    public interface IUrlContext
    {
        bool Matches(IUrlContext urlContext);
    }
}
