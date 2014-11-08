using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CH.Website.Utility.FluentHtml.Elements;

namespace CH.Website.Utility.FluentHtml
{
    public static class ElementFactoryExtensions
    {
        public static FormElement Form(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<FormElement>();
        }

        public static LabelElement Label(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<LabelElement>();
        }

        public static LabelElement<T> Label<T>(this ElementFactory<T> elementFactory)
        {
            return elementFactory.CreateElement<LabelElement<T>>();
        }

        public static LinkElement Link(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<LinkElement>();
        }

        public static ListItemElement ListItem(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<ListItemElement>();
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

        public static InputHiddenElement<T> InputHidden<T>(this ElementFactory<T> elementFactory)
        {
            return elementFactory.CreateElement<InputHiddenElement<T>>();
        }

        public static InputPasswordElement InputPassword(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<InputPasswordElement>();
        }

        public static InputPasswordElement<T> InputPassword<T>(this ElementFactory<T> elementFactory)
        {
            return elementFactory.CreateElement<InputPasswordElement<T>>();
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

        public static InputTextElement<T> InputText<T>(this ElementFactory<T> elementFactory)
        {
            return elementFactory.CreateElement<InputTextElement<T>>();
        }

        public static UnorderedListElement UnorderedList(this ElementFactory elementFactory)
        {
            return elementFactory.CreateElement<UnorderedListElement>();
        }
    }
}