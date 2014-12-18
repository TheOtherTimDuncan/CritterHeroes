using System;
using System.Collections.Generic;
using CH.Website.Utility;

namespace CH.Website.Models.Modal
{
    public class ModalDialogButton
    {
        private ModalDialogButton()
        {
        }

        public static ModalDialogButton Button(string text, ButtonCss cssClass, bool isDismissable)
        {
            return new ModalDialogButton()
            {
                ButtonType = ModalDialogButtonType.Button,
                Text = text,
                CssClass = cssClass,
                IsDismissable = isDismissable
            };
        }

        public static ModalDialogButton Link(string text, ButtonCss cssClass, string url)
        {
            return new ModalDialogButton()
            {
                ButtonType = ModalDialogButtonType.Link,
                Text = text,
                CssClass = cssClass,
                Url = url
            };
        }

        public ModalDialogButtonType ButtonType
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public bool IsDismissable
        {
            get;
            set;
        }

        public ButtonCss CssClass
        {
            get;
            set;
        }
    }
}