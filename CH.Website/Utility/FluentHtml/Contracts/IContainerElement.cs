using System;
using System.Collections.Generic;

namespace CH.Website.Utility.FluentHtml.Contracts
{
    public interface IContainerElement : IElement
    {
        IEnumerable<IElement> Children
        {
            get;
        }
    }
}
