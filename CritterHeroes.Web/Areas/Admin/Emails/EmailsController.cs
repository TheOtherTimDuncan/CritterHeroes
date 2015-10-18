using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Areas.Admin.Emails
{
    [Authorize(Roles = UserRole.MasterAdmin)]
    [Route("Emails/{action=index}")]
    public class EmailsController : BaseAdminController
    {
        public EmailsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
