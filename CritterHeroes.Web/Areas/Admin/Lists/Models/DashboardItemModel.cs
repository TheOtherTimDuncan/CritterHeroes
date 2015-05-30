using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Admin.Lists.Models
{
    public class DashboardItemModel
    {
        public DashboardItemModel(int id, string title)
        {
            this.ID = id;
            this.Title = title;
        }

        public int ID
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }
    }
}