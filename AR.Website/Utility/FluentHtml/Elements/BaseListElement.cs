using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TOTD.Utility.Misc;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class BaseListElement<T> : BaseContainerElement<T> where T : BaseListElement<T>
    {
        public BaseListElement(ListType listType, ViewContext viewContext)
            : base(listType.Tag, viewContext)
        {
        }

        public class ListType : Enumeration
        {
            public static ListType Ordered = new ListType(0, "ol");
            public static ListType Unordered = new ListType(1, "ul");

            public string Tag
            {
                get
                {
                    return this.DisplayName;
                }
            }

            private ListType()
            {
            }

            private ListType(int value, string displayName)
                : base(value, displayName)
            {
            }
        }
    }
}