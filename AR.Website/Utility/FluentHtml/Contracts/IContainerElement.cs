using System;
using System.Collections.Generic;

namespace AR.Website.Utility.FluentHtml.Contracts
{
    public interface IContainerElement : IElement
    {
        IEnumerable<IElement> Children
        {
            get;
        }
    }
}
