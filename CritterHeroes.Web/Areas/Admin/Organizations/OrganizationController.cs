using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Organizations.Models;
using CritterHeroes.Web.Areas.Admin.Organizations.Queries;
using CritterHeroes.Web.Common.ActionResults;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Admin.Organizations
{
    [Route("Organization/{action=index}")]
    public class OrganizationController : BaseAdminController
    {
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
                    // If we don't have an uploaded logo this must be a normal post
                    // so we can do a normal redirect
                    if (model.LogoFile == null)
                    {
                        return RedirectToPrevious();
                    };

                    // If we do have an uploaded logo, this is an ajax post
                    return Json(new
                    {
                        Succeeded = true,
                        Url = Url.Content("~/")
                    });
                }
                else
                {
                    AddCommandResultErrorsToModelState(ModelState, commandResult);
                }
            }

            return View(model);
        }
    }
}