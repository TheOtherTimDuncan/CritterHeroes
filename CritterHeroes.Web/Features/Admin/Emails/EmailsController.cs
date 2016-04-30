using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Admin.Emails.Models;
using CritterHeroes.Web.Features.Admin.Emails.Queries;

namespace CritterHeroes.Web.Features.Admin.Emails
{
    [Authorize(Roles = UserRole.MasterAdmin)]
    [Route(EmailsController.Route + "/{action=index}")]
    public class EmailsController : BaseAdminController
    {
        public const string Route = "Emails";

        public EmailsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        public async Task<ActionResult> Index(EmailQuery query)
        {
            EmailModel model = await QueryDispatcher.DispatchAsync(query);
            return View(model);
        }
    }
}
