using System;
using System.Collections.Generic;

namespace CH.Website.Utility.FluentHtml.Contracts
{
    public interface IHtmlConvention
    {
        void ApplyConvention(IElement element);
    }
}
