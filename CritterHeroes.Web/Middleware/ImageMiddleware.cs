using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using Microsoft.Owin;
using Owin;
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
            if (context.Request.Path.StartsWithSegments(_path))
            {
                int segmentCount = context.Request.Uri.Segments.Length;

                string critterIDSegment = context.Request.Uri.Segments[segmentCount - 2];
                critterIDSegment = critterIDSegment.Substring(0, critterIDSegment.Length - 1);

                int critterID;
                if (int.TryParse(critterIDSegment, out critterID))
                {
                    string filename = context.Request.Uri.Segments[segmentCount - 1];

                    ISqlStorageContext<Critter> critterStorage = _dependencyResolver.GetService<ISqlStorageContext<Critter>>();
                    var critter = await critterStorage.Entities
                        .MatchingID(critterID)
                        .SelectMany(x => x.Pictures)
                        .Where(x => x.Picture.Filename == filename)
                        .Select(x => new
                        {
                            x.Picture.ContentType,
                            x.Picture.FileSize,
                            x.Picture.WhenCreated
                        }).SingleOrDefaultAsync();

                    if (critter != null)
                    {
                        ICritterPictureService pictureService = _dependencyResolver.GetService<ICritterPictureService>();
                        await pictureService.GetPictureAsync(critterID, filename, context.Response.Body);
                        context.Response.ContentLength = critter.FileSize;
                        context.Response.ContentType = critter.ContentType;
                        context.Response.Headers["Last-Modified"] = critter.WhenCreated.ToUniversalTime().ToString("r");
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }
}
