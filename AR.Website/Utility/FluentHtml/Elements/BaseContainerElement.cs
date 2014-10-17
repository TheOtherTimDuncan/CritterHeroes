using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class BaseContainerElement<T> : Element<T>, IContainerElement, IDisposable where T : BaseContainerElement<T>
    {
        private List<IElement> _children;
        private bool needsEnd;
        private bool isDisposed;

        public BaseContainerElement(string tag, HtmlHelper htmlHelper)
            : base(tag, htmlHelper)
        {
            _children = new List<IElement>();
            needsEnd = false;
        }

        public IEnumerable<IElement> Children
        {
            get
            {
                return _children;
            }
        }

        public T AddElement(IElement element)
        {
            AddInnerHtml(element);
            _children.Add(element);
            return (T)this;
        }

        public T AddHtml(MvcHtmlString html)
        {
            AddInnerHtml(html);
            return (T)this;
        }

        public T AddElement(Func<IElement> elementBuilder)
        {
            IElement element = elementBuilder();
            return AddElement(element);
        }

        public T Begin()
        {
            needsEnd = true;
            ViewWriter.Write(Builder.ToString(TagRenderMode.StartTag));
            return (T)this;
        }

        public void End()
        {
            if (needsEnd)
            {
                ViewWriter.Write(Builder.ToString(TagRenderMode.EndTag));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                End();
            }
        }
    }
}