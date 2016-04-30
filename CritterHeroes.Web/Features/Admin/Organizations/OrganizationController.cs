using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Admin.Organizations.Models;
using CritterHeroes.Web.Features.Admin.Organizations.Queries;

namespace CritterHeroes.Web.Features.Admin.Organizations
{
    [Route(OrganizationController.Route + "/{action=index}")]
    public class OrganizationController : BaseAdminController
    {
        public const string Route = "Organization";

        public OrganizationController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        public async Task<ActionResult> EditProfile()
        {
            EditProfileModel model = await QueryDispatcher.DispatchAsync(new EditProfileQuery());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(EditProfileModel model)
        {
            if (ModelState.IsValid)
            {
                CommandResult commandResult = await CommandDispatcher.DispatchAsync(model);
                if (commandResult.Succeeded)
                {
                    return RedirectToPrevious();
                }
                else
                {
                    if (Request.IsAjaxRequest())
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, string.Join(". ", commandResult.Errors));
                    }
                    AddCommandResultErrorsToModelState(ModelState, commandResult);
                }
            }

            return View(model);
        }
    }
}
