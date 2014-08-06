using System;
using System.Collections.Generic;

namespace AR.Website.Utility.FluentHtml.Contracts
{
    public interface IHtmlConvention
    {
        void ApplyConvention(IElement element);
    }
}
