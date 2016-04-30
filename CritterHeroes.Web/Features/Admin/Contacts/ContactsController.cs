using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Admin.Contacts.Commands;
using CritterHeroes.Web.Features.Admin.Contacts.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Queries;
using CritterHeroes.Web.Features.Common;

namespace CritterHeroes.Web.Features.Admin.Contacts
{
    [Route(ContactsController.Route + "/{action=index}")]
    public class ContactsController : BaseAdminController
    {
        public const string Route = "Contacts";

        public ContactsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        [AuthorizeRoles(UserRole.MasterAdmin, UserRole.Admin)]
        public async Task<ActionResult> Index(ContactsQuery query)
        {
            ContactsModel model = await QueryDispatcher.DispatchAsync(query);
            return View(model);
        }

        [AuthorizeRoles(UserRole.MasterAdmin, UserRole.Admin)]
        public async Task<ActionResult> List(ContactsListQuery query)
        {
            ContactsListModel model = await QueryDispatcher.DispatchAsync(query);
            return JsonCamelCase(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.MasterAdmin)]
        public async Task<ActionResult> ImportPeople(ImportPeopleCommand command)
        {
            await CommandDispatcher.DispatchAsync(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.MasterAdmin)]
        public async Task<ActionResult> ImportBusinesses(ImportBusinessCommand command)
        {
            await CommandDispatcher.DispatchAsync(command);
            return RedirectToAction("Index");
        }
    }
}
