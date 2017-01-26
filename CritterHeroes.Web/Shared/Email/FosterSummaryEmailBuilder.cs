using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Email;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.StateManagement;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public class FosterSummaryEmailBuilder : EmailBuilderBase<FosterSummaryEmailCommand>
    {
        public FosterSummaryEmailBuilder(IUrlGenerator urlGenerator, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService, IEmailConfiguration emailConfiguration)
            : base(urlGenerator, stateManager, logoService, emailConfiguration)
        {
        }

        protected override EmailBuilder BuildEmail(EmailBuilder builder, FosterSummaryEmailCommand command)
        {
            EmailBuilder result = builder
                .BeginTable(className: "critters-list")
                    .BeginTableRow()
                        .AddTableHeader("Name")
                        .AddTableHeader("Baby")
                        .AddTableHeader("Young")
                        .AddTableHeader("Adult")
                        .AddTableHeader("Senior")
                    .EndTableRow();

            foreach (var summary in command.Summaries)
            {
                builder.BeginTableRow();
                builder.AddTableCell(summary.FosterName);
                builder.AddTableCell(summary.BabyCount.ToString());
                builder.AddTableCell(summary.YoungCount.ToString());
                builder.AddTableCell(summary.AdultCount.ToString());
                builder.AddTableCell(summary.SeniorCount.ToString());
                builder.EndTableRow();
            }

            return result = result
                .EndTable()
                .WithSubject($"Critters Summary - {command.OrganizationFullName}");
        }
    }
}
