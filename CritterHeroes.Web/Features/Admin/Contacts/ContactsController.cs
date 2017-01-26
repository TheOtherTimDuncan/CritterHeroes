using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Domain.Contracts.Commands;
using CritterHeroes.Web.Domain.Contracts.Queries;
using CritterHeroes.Web.Features.Admin.Contacts.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Queries;
using CritterHeroes.Web.Features.Shared;

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

        [AuthorizeRoles(UserRole.MasterAdmin, UserRole.Admin)]
        [Route("Person/{" + nameof(PersonEditQuery.PersonID) + "}")]
        public async Task<ActionResult> Person(PersonEditQuery query)
        {
            PersonEditModel model = await QueryDispatcher.DispatchAsync(query);
            return View(model);
        }

        [AuthorizeRoles(UserRole.MasterAdmin, UserRole.Admin)]
        [Route("Business/{" + nameof(BusinessEditQuery.BusinessID) + "}")]
        public async Task<ActionResult> Business(BusinessEditQuery query)
        {
            BusinessEditModel model = await QueryDispatcher.DispatchAsync(query);
            return View(model);
        }
    }
}
