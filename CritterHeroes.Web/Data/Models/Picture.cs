using System;
using System.Collections.Generic;
using System.Linq;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Data.Models
{
    public class Picture
    {
        protected Picture()
        {
        }

        public Picture(string filename, int width, int height, long fileSize, string contentType)
        {
            ThrowIf.Argument.IsNullOrEmpty(filename, nameof(filename));
            ThrowIf.Argument.IsNullOrEmpty(contentType, nameof(contentType));

            this.Filename = filename;
            this.ContentType = contentType;
            this.Width = width;
            this.Height = height;
            this.FileSize = fileSize;

            this.WhenCreated = DateTimeOffset.UtcNow;
            this.ChildPictures = new List<PictureChild>();
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

        public int? DisplayOrder
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

        public string ContentType
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

        public string RescueGroupsID
        {
            get;
            set;
        }

        public virtual ICollection<PictureChild> ChildPictures
        {
            get;
            private set;
        }

        public PictureChild AddChildPicture(int width, int height, int fileSize)
        {
            PictureChild child = new PictureChild(this, $"{Filename}_{width}x{height}", width, height, fileSize);
            ChildPictures.Add(child);
            return child;
        }
    }
}
