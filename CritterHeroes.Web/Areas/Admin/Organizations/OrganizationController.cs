using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Organizations.Models;
using CritterHeroes.Web.Areas.Admin.Organizations.Queries;
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
    }
}