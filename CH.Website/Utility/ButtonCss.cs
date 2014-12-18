using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.Misc;

namespace CH.Website.Utility
{
    public class ButtonCss : Enumeration<ButtonCss>
    {
        public static readonly ButtonCss Default = new ButtonCss(0, "btn-default");
        public static readonly ButtonCss Primary = new ButtonCss(1, "btn-primary");
        public static readonly ButtonCss Success = new ButtonCss(2, "btn-success");
        public static readonly ButtonCss Info = new ButtonCss(3, "btn-info");
        public static readonly ButtonCss Warning = new ButtonCss(4, "btn-warning");
        public static readonly ButtonCss Danger = new ButtonCss(5, "btn-danger");

        protected ButtonCss(int value, string className)
            : base(value, className)
        {
        }

        public string ClassName
        {
            get
            {
                return this.DisplayName;
            }
        }
    }
}