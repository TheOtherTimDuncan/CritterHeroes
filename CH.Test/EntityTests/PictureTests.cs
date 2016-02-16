using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.EntityFramework;

namespace CH.Test.EntityTests
{
    [TestClass]
    public class PictureTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanReadWriteAndDeletePicture()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            Picture picture = new Picture("filename", 0, 0, 0, "contenttype");

            using (TestSqlStorageContext<Picture> storageContext = new TestSqlStorageContext<Picture>())
            {
                storageContext.FillWithTestData(picture, "ID");
                storageContext.Add(picture);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Picture> storageContext = new TestSqlStorageContext<Picture>())
            {
                Picture result = await storageContext.Entities.FindByIDAsync(picture.ID);
                result.Should().NotBeNull();

                result.Filename.Should().Be(picture.Filename);
                result.ContentType.Should().Be(picture.ContentType);
                result.DisplayOrder.Should().Be(picture.DisplayOrder);
                result.Width.Should().Be(picture.Width);
                result.Height.Should().Be(picture.Height);
                result.FileSize.Should().Be(picture.FileSize);
                result.RescueGroupsID.Should().Be(picture.RescueGroupsID);
                result.WhenCreated.Should().Be(picture.WhenCreated);
                result.RescueGroupsCreated.Should().Be(picture.RescueGroupsCreated);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.Entities.SingleOrDefault(x => x.ID == picture.ID).Should().BeNull();
            }
        }

        [TestMethod]
        public async Task CanReadWriteAndDeletePictureChild()
        {
            Picture picture = new Picture("test.jpg", 0, 0, 0, "contenttype");
            PictureChild child1 = picture.AddChildPicture(0, 0, 0);
            PictureChild child2 = picture.AddChildPicture(0, 0, 0);

            using (TestSqlStorageContext<Picture> storageContext = new TestSqlStorageContext<Picture>())
            {
                storageContext.FillWithTestData(child1, "ParentID");
                storageContext.FillWithTestData(child2, "ParentID");

                storageContext.Add(picture);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Picture> storageContext = new TestSqlStorageContext<Picture>())
            {
                Picture result = await storageContext.Entities.FindByIDAsync(picture.ID);
                result.Should().NotBeNull();

                result.ChildPictures.Should().HaveCount(2);

                PictureChild result1 = result.ChildPictures.SingleOrDefault(x => x.ID == child1.ID);
                result1.Should().NotBeNull();

                result1.Filename.Should().Be(child1.Filename);
                result1.Width.Should().Be(child1.Width);
                result1.Height.Should().Be(child1.Height);
                result1.FileSize.Should().Be(child1.FileSize);
                result1.WhenCreated.Should().Be(child1.WhenCreated);

                result1.ParentID.Should().Be(result.ID);
                result1.Parent.Should().NotBeNull();
                result1.Parent.ID.Should().Be(result.ID);

                PictureChild result2 = result.ChildPictures.SingleOrDefault(x => x.ID == child2.ID);
                result2.Should().NotBeNull();

                result2.Filename.Should().Be(child2.Filename);
                result2.Width.Should().Be(child2.Width);
                result2.Height.Should().Be(child2.Height);
                result2.FileSize.Should().Be(child2.FileSize);
                result2.WhenCreated.Should().Be(child2.WhenCreated);

                result2.ParentID.Should().Be(result.ID);
                result2.Parent.Should().NotBeNull();
                result2.Parent.ID.Should().Be(result.ID);

                // Can child be removed from person?
                result.ChildPictures.Remove(result2);
                await storageContext.SaveChangesAsync();
            }
        }

        [TestMethod]
        public void PictureCreatesChildWithFilenameBasedOnPictureFilename()
        {
            Picture picture = new Picture("filename.jpg", 1, 1, 1, "contenttype");
            PictureChild child = picture.AddChildPicture(100, 200, 300);
            child.Filename.Should().Be("filename_100x200.jpg");
        }
    }
}
