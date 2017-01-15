using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public class FosterSummaryEmailBuilder : EmailBuilderBase<FosterSummaryEmailCommand, FosterSummaryEmailCommand.FosterSummaryEmailData>
    {
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

            foreach (var summary in command.EmailData.Critters)
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
                .WithSubject($"Critters Summary - {command.EmailData.OrganizationFullName}");
        }
    }
}
