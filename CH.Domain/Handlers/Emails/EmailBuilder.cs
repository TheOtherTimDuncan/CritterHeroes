using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models;

namespace CH.Domain.Handlers.Emails
{
    public class EmailBuilder
    {
        private EmailMessage _message;

        private StringBuilder _html;
        private StringBuilder _text;

        private const string _style = "style='font-family: \"Open Sans\", \"Helvetica Neue\", Helvetica, Arial, sans-serif;'";

        private EmailBuilder(EmailMessage message)
        {
            _html = new StringBuilder();
            _html.Append("<html>");
            _html.Append("<body>");

            _text = new StringBuilder();

            _message = message;
        }

        public static EmailBuilder Begin(EmailMessage message)
        {
            return new EmailBuilder(message);
        }

        public EmailBuilder AddParagraph(string text)
        {
            _text.AppendLine(text);
            _html.AppendFormat("<p {0}>{1}</p>", _style, text);
            return this;
        }

        public void End()
        {
            _html.Append("</body></html>");
            _message.HtmlBody = _html.ToString();
            _message.TextBody = _text.ToString();
        }
    }
}
