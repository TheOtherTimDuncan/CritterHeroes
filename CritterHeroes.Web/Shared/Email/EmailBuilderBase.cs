using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Email;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Features.Shared.ActionExtensions;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.StateManagement;
using TOTD.Mailer.Core;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Shared.Email
{
    public abstract class EmailBuilderBase<TCommand> : IEmailBuilder<TCommand> where TCommand : EmailCommandBase
    {
        private IStateManager<OrganizationContext> _stateManager;
        private IOrganizationLogoService _logoService;
        private IUrlGenerator _urlGenerator;
        private IEmailConfiguration _emailConfiguration;

        private const string _styles = @"
    .critters-list {
      width: 100%;
      border-collapse: collapse;
      margin-bottom: 15px;
    }
    .critters-list th {
      text-align: center;
    }
    .critters-list img{
        width: 50px;
    }
    .critters-list td,.critters-list th {
      padding: 5px 5px;
      text-align: left;
      border: darkgray 1px solid;
    }
";
        public EmailBuilderBase(IUrlGenerator urlGenerator, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService, IEmailConfiguration emailConfiguration)
        {
            this._urlGenerator = urlGenerator;
            this._stateManager = stateManager;
            this._logoService = logoService;
            this._emailConfiguration = emailConfiguration;
        }

        protected abstract EmailBuilder BuildEmail(EmailBuilder builder, TCommand command);

        public EmailMessage BuildEmail(TCommand command)
        {
            OrganizationContext organizationContext = _stateManager.GetContext();
            command.OrganizationFullName = organizationContext.FullName;
            command.OrganizationShortName = organizationContext.ShortName;

            command.UrlLogo = _logoService.GetLogoUrl();

            command.UrlHome = _urlGenerator.GenerateAbsoluteHomeUrl();

            EmailBuilder builder = EmailBuilder.Begin();

            if (!command.EmailCc.IsNullOrEmpty())
            {
                builder.Cc(command.EmailCc);
            }

            EmailMessage emailMessage = BuildEmail(builder, command)
                .AddStyles(_styles)
                .BeginParagraph()
                    .AddText("Thanks,")
                    .AddLineBreak()
                    .AddText(command.OrganizationFullName)
                    .AddLineBreak()
                    .AddLink(command.UrlHome, command.UrlHome)
                .EndParagraph()
                .AddImage(command.UrlLogo, "Logo")
                .To(command.EmailTo)
                .From(command.EmailFrom ?? _emailConfiguration.DefaultFrom)
                .ToEmail();

            return emailMessage;
        }
    }
}
