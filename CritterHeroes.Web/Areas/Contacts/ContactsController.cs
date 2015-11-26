using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Areas.Contacts.Models;
using CritterHeroes.Web.Areas.Contacts.Queries;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Areas.Contacts
{
    [AuthorizeRoles(UserRole.MasterAdmin, UserRole.Admin)]
    [Route("Contacts/{action=index}")]
    public class ContactsController : BaseController
    {
        public ContactsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        public async Task<ActionResult> Index(ContactsQuery query)
        {
            ContactsModel model = await QueryDispatcher.DispatchAsync(query);
            return View(model);
        }
    }
}
