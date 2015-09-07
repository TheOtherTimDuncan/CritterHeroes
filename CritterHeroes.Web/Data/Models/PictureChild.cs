using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class PictureChild
    {
        protected PictureChild()
        {
        }

        internal PictureChild(Picture parent, string filename, int width, int height, long fileSize)
        {
            ThrowIf.Argument.IsNull(parent, nameof(parent));
            ThrowIf.Argument.IsNullOrEmpty(filename, nameof(filename));

            this.ParentID = parent.ID;
            this.Parent = parent;

            this.Filename = filename;
            this.Width = width;
            this.Height = height;
            this.FileSize = fileSize;

            this.WhenCreated = DateTimeOffset.UtcNow;
        }

        public int ID
        {
            get;
            private set;
        }

        public string Filename
        {
            get;
            set;
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public long FileSize
        {
            get;
            private set;
        }

        public DateTimeOffset WhenCreated
        {
            get;
            private set;
        }

        public DateTime? RescueGroupsCreated
        {
            get;
            set;
        }

        public int ParentID
        {
            get;
            private set;
        }

        public virtual Picture Parent
        {
            get;
            private set;
        }
    }
}
