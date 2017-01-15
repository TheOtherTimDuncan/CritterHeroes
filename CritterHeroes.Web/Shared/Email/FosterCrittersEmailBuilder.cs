using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Shared.Email
{
    public class FosterCrittersEmailBuilder : EmailBuilderBase<FosterCrittersEmailCommand, FosterCrittersEmailCommand.FosterCrittersEmailData>
    {
        protected override EmailBuilder BuildEmail(EmailBuilder builder, FosterCrittersEmailCommand command)
        {
            EmailBuilder result = builder
                .AddParagraph($"According to {command.EmailData.OrganizationShortName} records, the following critters are currently in your care. If any of this information is out of date, please respond to this email with what needs changed.")
                .BeginTable(className: "critters-list")
                    .BeginTableRow()
                        .AddTableHeader(String.Empty)
                        .AddTableHeader("Name")
                        .AddTableHeader("Status")
                        .AddTableHeader("Sex")
                        .AddTableHeader("ID")
                        .AddTableHeader(String.Empty)
                    .EndTableRow();

            foreach (var data in command.EmailData.Critters)
            {
                builder.BeginTableRow();

                if (!data.ThumbnailUrl.IsNullOrEmpty())
                {
                    builder
                        .BeginTableCell()
                        .AddImage(data.ThumbnailUrl, "Thumbnail")
                        .EndTableCell();
                }
                else
                {
                    builder.AddTableCell(String.Empty);
                }

                builder.AddTableCell(data.Name);
                builder.AddTableCell(data.Status);
                builder.AddTableCell(data.Sex);
                builder.AddTableCell(data.RescueID);

                builder.
                    BeginTableCell()
                        .AddLink($"http://www.fflah.org/animals/detail?AnimalID={data.RescueGroupsID}", "View")
                    .EndTableCell();

                builder.EndTableRow();
            }

            result = result
                .EndTable()
                .WithSubject($"Critters Update - {command.EmailData.OrganizationFullName}");

            return result;
        }
    }
}
