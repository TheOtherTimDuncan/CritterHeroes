using System;
using System.Collections.Generic;

namespace AR.Website.Areas.Admin.Models
{
    public class DashboardItemModel
    {
        public DashboardItemModel(string description, string statusUrl)
        {
            this.Title = description;
            this.StatusUrl = statusUrl;
        }

        public string Title
        {
            get;
            set;
        }

        public string StatusUrl
        {
            get;
            set;
        }
    }
}