using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Models.Modal
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