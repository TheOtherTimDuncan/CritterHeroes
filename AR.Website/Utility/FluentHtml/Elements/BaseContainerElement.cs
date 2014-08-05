using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class BaseContainerElement<T> : Element<T>, IDisposable, IContainerElement where T : BaseContainerElement<T>
    {
        private List<IElement> _children;
        public bool _isDisposed;

        public BaseContainerElement(string tag, ViewContext viewContext)
            : base(tag, viewContext)
        {
            _children = new List<IElement>();
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

        public T AddElement(Func<IElement> elementBuilder)
        {
            IElement element = elementBuilder();
            return AddElement(element);
        }

        public T Begin()
        {
            ViewWriter.Write(Builder.ToString(TagRenderMode.StartTag));
            if (innerHtmlBuilder.Length > 0)
            {
                ViewWriter.Write(innerHtmlBuilder.ToString());
            }
            return (T)this;
        }

        public void End()
        {
            ViewWriter.Write(Builder.ToString(TagRenderMode.EndTag));
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                End();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}