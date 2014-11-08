using System;
using System.Collections.Generic;
using System.Web;

namespace CH.Website.Utility.FluentHtml.Contracts
{
    public interface IElement : IHtmlString
    {
        //IElement Attribute(string name, object value);
        //IElement Class(string className);

        string CurrentArea
        {
            get;
        }

        string CurrentControllerName
        {
            get;
        }

        string CurrentActionName
        {
            get;
        }
    }
}
