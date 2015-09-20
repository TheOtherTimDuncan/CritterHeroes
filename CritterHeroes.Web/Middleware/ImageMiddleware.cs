using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using Microsoft.Owin;
using Owin;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Middleware
{
    public static class ImageMiddlewareExtensions
    {
        public static void UseImage(this IAppBuilder builder, IDependencyResolver dependencyResolver)
        {
            builder.Use<ImageMiddleware>(dependencyResolver);
        }
    }

    public class ImageMiddleware : OwinMiddleware
    {
        public const string Route = "image/critter";

        private IDependencyResolver _dependencyResolver;

        private static readonly PathString _path = new PathString("/" + Route);

        public ImageMiddleware(OwinMiddleware next, IDependencyResolver dependencyResolver)
            : base(next)
        {
            ThrowIf.Argument.IsNull(dependencyResolver, nameof(dependencyResolver));
            this._dependencyResolver = dependencyResolver;
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (!context.Request.Path.StartsWithSegments(_path))
            {
                // If this isn't the target of the request then let's move on
                await Next.Invoke(context);
                return;
            }

            int segmentCount = context.Request.Uri.Segments.Length;

            // Critter ID should be an integer and in the url segment before the filename

            string critterIDSegment = context.Request.Uri.Segments[segmentCount - 2];
            critterIDSegment = critterIDSegment.Substring(0, critterIDSegment.Length - 1);

            int critterID;
            if (!int.TryParse(critterIDSegment, out critterID))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            string filename = context.Request.Uri.Segments[segmentCount - 1];
            ICritterPictureService pictureService = _dependencyResolver.GetService<ICritterPictureService>();

            if (context.Request.Query.IsNullOrEmpty())
            {
                // If no width or height reqeusted then redirect to the requested image
                string url = pictureService.GetPictureUrl(critterID, filename);
                context.Response.Redirect(url);
                return;
            }

            ISqlStorageContext<Critter> critterStorage = _dependencyResolver.GetService<ISqlStorageContext<Critter>>();

            Critter critter = await critterStorage.Entities.FindByIDAsync(critterID);
            if (critter == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            CritterPicture critterPicture = critter.Pictures.SingleOrDefault(x => x.Picture.Filename == filename);
            if (critterPicture == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            int? width = null;
            int widthValue;
            if (int.TryParse(context.Request.Query["width"], out widthValue))
            {
                width = widthValue;
            }

            int? height = null;
            int heightValue;
            if (int.TryParse(context.Request.Query["height"], out heightValue))
            {
                height = heightValue;
            }

            // If width and height are missing or invalid then we'll ignore the parameters
            // and redirect to the default image
            if (width == null && height == null)
            {

                string url = pictureService.GetPictureUrl(critterID, filename);
                context.Response.Redirect(url);
                return;
            }

            // Calculate the proportional width and height for the requested parameters

            float widthRatio = critterPicture.Picture.Width / (width ?? critterPicture.Picture.Width);
            float heightRatio = critterPicture.Picture.Height / (height ?? critterPicture.Picture.Height);
            float ratio = (heightRatio > widthRatio ? heightRatio : widthRatio);

            int proportionalWidth = Convert.ToInt32(Math.Floor(critterPicture.Picture.Width / ratio));
            int proportionalHeight = Convert.ToInt32(Math.Floor(critterPicture.Picture.Height / ratio));

            PictureChild childPicture = critterPicture.Picture.ChildPictures.SingleOrDefault(x => (x.Width == proportionalWidth) && (x.Height == proportionalHeight));
            if (childPicture != null)
            {
                // If existing picture found then redirect to that picture
                string url = pictureService.GetPictureUrl(critterID, childPicture.Filename);
                context.Response.Redirect(url);
                return;
            }

            // No image found matching requested width and height so let's create one

            using (MemoryStream stream = new MemoryStream())
            {
                await pictureService.GetPictureAsync(critterID, filename, stream);

                using (Image img = Image.FromStream(stream, useEmbeddedColorManagement: true, validateImageData: false))
                {
                    using (Image thumb = img.GetThumbnailImage(proportionalWidth, proportionalHeight, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero))
                    {
                        using (MemoryStream resized = new MemoryStream())
                        {
                            thumb.Save(resized, img.RawFormat);

                            childPicture = critterPicture.Picture.AddChildPicture(proportionalWidth, proportionalHeight, resized.Length);
                            await critterStorage.SaveChangesAsync();

                            resized.Position = 0;
                            await pictureService.SavePictureAsync(resized, critterID, childPicture.Filename, critterPicture.Picture.ContentType);

                            resized.Position = 0;
                            await resized.CopyToAsync(context.Response.Body);
                            context.Response.ContentType = critterPicture.Picture.ContentType;
                            context.Response.ContentLength = resized.Length;
                        }
                    }
                }
            }
        }

        private bool ThumbnailCallback()
        {
            return false;
        }
    }
}
