using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Common.Email
{
    public class EmailBuilder
    {
        private EmailMessage _message;

        private StringBuilder _html;
        private StringBuilder _text;

        private const string _style = "style='font-family: \"Open Sans\", \"Helvetica Neue\", Helvetica, Arial, sans-serif;'";
        private const string _tableStyle = "style='font-family: \"Open Sans\", \"Helvetica Neue\", Helvetica, Arial, sans-serif; padding: 3px; border: 1px solid #333; border-collapse: collapse;'";

        private EmailBuilder(EmailMessage message)
        {
            _html = new StringBuilder();
            _html.AppendLine("<html>");
            _html.AppendLine("<body>");

            _text = new StringBuilder();

            _message = message;
        }

        public static EmailBuilder Begin(EmailMessage message)
        {
            return new EmailBuilder(message);
        }

        public EmailBuilder SetNotificationSubject(OrganizationContext organizationContext)
        {
            _message.Subject = organizationContext.ShortName + " Admin Notification";
            return this;
        }

        public EmailBuilder AddParagraph(string text)
        {
            _text.AppendLine(text);
            _html.AppendLine($"<p {_style}>{text}</p>");
            return this;
        }

        public EmailBuilder StartTable()
        {
            _html.AppendLine($"<table {_tableStyle}>");
            return this;
        }

        public EmailBuilder EndTable()
        {
            _html.AppendLine("</table>");
            return this;
        }

        public EmailBuilder StartTableRow()
        {
            _html.AppendLine($"<tr {_style}>");
            return this;
        }

        public EmailBuilder EndTableRow()
        {
            _html.AppendLine("</tr>");
            _text.AppendLine();
            return this;
        }

        public EmailBuilder AddTableHeader(string contents)
        {
            _html.Append($"<th {_tableStyle}>{contents}</th>");
            _text.Append(contents);
            _text.Append("\t");
            return this;
        }

        public EmailBuilder AddTableCell(string contents)
        {
            _html.Append($"<td {_tableStyle}>{contents}</td>");
            _text.Append(contents);
            _text.Append("\t");
            return this;
        }

        public EmailBuilder Repeat<T>(IEnumerable<T> source, Action<T, EmailBuilder> action)
        {
            foreach (T element in source)
            {
                action(element, this);
            }
            return this;
        }

        public void End()
        {
            _html.AppendLine("</body>");
            _html.AppendLine("</html>");
            _message.HtmlBody = _html.ToString();
            _message.TextBody = _text.ToString();
        }
    }
}
