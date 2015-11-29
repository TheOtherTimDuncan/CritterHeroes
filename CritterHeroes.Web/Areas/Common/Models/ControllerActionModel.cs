using System;

namespace CritterHeroes.Web.Areas.Common.Models
{
    public class ControllerActionModel
    {
        public string ControllerRoute
        {
            get;
            set;
        }

        public string ActionName
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string AreaName
        {
            get;
            set;
        }
    }
}
