using System;
using System.Collections.Generic;
using System.Web;

namespace CritterHeroes.Web.Areas.Admin.Critters.Commands
{
    public class UploadCsvFileCommand
    {
        public HttpPostedFileBase File
        {
            get;
            set;
        }
    }
}
