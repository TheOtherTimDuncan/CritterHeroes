using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Models.Modal;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Commands;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Areas.Account.Commands
{
    public class ForgotPasswordCommand : IUserCommand
    {
        public ForgotPasswordCommand(ForgotPasswordModel model, OrganizationContext organizationContext)
        {
            ThrowIf.Argument.IsNull(model, "model");
            ThrowIf.Argument.IsNull(organizationContext, "organizationContext");

            this.Model = model;
            this.OrganizationContext = organizationContext;
        }

        public ForgotPasswordModel Model
        {
            get;
            private set;
        }

        public OrganizationContext OrganizationContext
        {
            get;
            private set;
        }

        public ModalDialogModel ModalDialog
        {
            get;
            set;
        }

        public string Username
        {
            get
            {
                return Model.Username;
            }
        }
    }
}