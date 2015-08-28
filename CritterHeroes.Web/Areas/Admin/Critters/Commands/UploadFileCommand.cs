using System;
using System.Collections.Generic;
using System.Web;

namespace CritterHeroes.Web.Areas.Admin.Critters.Commands
{
    public class UploadFileCommand
    {
        public HttpPostedFileBase File
        {
            get;
            set;
        }
    }
}
