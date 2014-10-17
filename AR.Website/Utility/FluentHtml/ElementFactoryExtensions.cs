using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;

namespace AR.Website.Utility.FluentHtml
{
    public static class ElementFactoryExtensions
    {
        public static FormElement Form(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<FormElement>();
        }

        public static LinkElement Link(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<LinkElement>();
        }

        public static ListItemElement ListItem(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<ListItemElement>();
        }

        public static UnorderedListElement UnorderedList(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<UnorderedListElement>();
        }

        public static SpanElement Span(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<SpanElement>();
        }

        public static InputButtonElement InputButton(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<InputButtonElement>();
        }

        public static InputCheckboxElement InputCheckbox(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<InputCheckboxElement>();
        }

        public static InputHiddenElement InputHidden(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<InputHiddenElement>();
        }

        public static InputPasswordElement InputPassword(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<InputPasswordElement>();
        }

        public static InputRadioElement InputRadio(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<InputRadioElement>();
        }

        public static InputSubmitElement InputSubmit(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<InputSubmitElement>();
        }

        public static InputTextElement InputText(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<InputTextElement>();
        }
    }
}