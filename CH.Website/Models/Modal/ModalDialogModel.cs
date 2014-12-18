using System;
using System.Collections.Generic;

namespace CH.Website.Models.Modal
{
    public class ModalDialogModel
    {
        public string Text
        {
            get;
            set;
        }

        public IEnumerable<ModalDialogButton> Buttons
        {
            get;
            set;
        }
    }
}