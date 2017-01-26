using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts;
using Moq;

namespace CH.Test.Mocks
{
    public class MockFileSystem : Mock<IFileSystem>
    {
        public MockFileSystem()
        {
            this.Setup(x => x.CombinePath(It.IsAny<string[]>())).Returns((string[] paths) => Path.Combine(paths));
            this.Setup(x => x.GetFileName(It.IsAny<string>())).Returns((string path) => Path.GetFileName(path));
            this.Setup(x => x.GetFileNameWithoutExtension(It.IsAny<string>())).Returns((string path) => Path.GetFileNameWithoutExtension(path));
            this.Setup(x => x.GetFileExtension(It.IsAny<string>())).Returns((string path) => Path.GetExtension(path));
        }
    }
}
