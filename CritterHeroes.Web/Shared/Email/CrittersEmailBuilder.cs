using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Shared.Email
{
    public class CrittersEmailBuilder : EmailBuilderBase<CrittersEmailCommand, CrittersEmailCommand.CrittersEmailData>
    {
        protected override EmailBuilder BuildEmail(EmailBuilder builder, CrittersEmailCommand command)
        {
            EmailBuilder result = builder
                .AddParagraph("The following are the critters in the status Available, Not Available or Sponsorship status.")
                .BeginTable(className: "critters-list")
                    .BeginTableRow()
                        .AddTableHeader(String.Empty)
                        .AddTableHeader("Name")
                        .AddTableHeader("Status")
                        .AddTableHeader("Sex")
                        .AddTableHeader("ID")
                        .AddTableHeader("Foster or Location")
                        .AddTableHeader(String.Empty)
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

                if (!data.Location.IsNullOrEmpty())
                {
                    builder.AddTableCell(data.Location);
                }
                else
                {
                    builder.BeginTableCell();
                    builder.AddText(data.FosterName);
                    if (!data.FosterEmail.IsNullOrEmpty())
                    {
                        builder.AddLineBreak(textAlternative: ", ");
                        builder.AddText(data.FosterEmail);
                    }
                }

                builder.
                    BeginTableCell()
                        .AddLink($"http://www.fflah.org/animals/detail?AnimalID={data.RescueGroupsID}", "View")
                    .EndTableCell()
                    .BeginTableCell()
                        .AddLink($"https://manage.rescuegroups.org/animal?animalID={data.RescueGroupsID}", "Edit")
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
