using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CritterHeroes.Web.Contracts;
using Moq;

namespace CH.Test.Mocks
{
    public class MockFileSystem:Mock<IFileSystem>
    {
        public MockFileSystem()
        {
            this.Setup(x => x.CombinePath(It.IsAny<string[]>())).Returns((string[] paths) => Path.Combine(paths));
        }
    }
}
