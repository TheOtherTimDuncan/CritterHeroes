using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Features.Common.Models;
using CritterHeroes.Web.Features.Common.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test
{
    [TestClass]
    public class PagingModelTests
    {
        private const int pageSize = 10;

        [TestMethod]
        public void ShouldCalculateCorrectPagingForResultOfLessThanOnePage()
        {
            PagingModel model = new PagingModel(pageSize - 1, new PagingQuery(1));
            model.PageSize = pageSize;

            model.TotalPages.Should().Be(1);
            model.CurrentPage.Should().Be(1);
            model.FirstPage.Should().Be(2);
            model.LastPage.Should().Be(0);
            model.PreviousPage.Should().Be(1);
            model.NextPage.Should().Be(1);
        }

        [TestMethod]
        public void ShouldCalculateCorrectPagingForResultOfTwoPages()
        {
            PagingModel model = new PagingModel(pageSize + 1, new PagingQuery(1));
            model.PageSize = pageSize;

            model.TotalPages.Should().Be(2);
            model.CurrentPage.Should().Be(1);
            model.FirstPage.Should().Be(2);
            model.LastPage.Should().Be(1);
            model.PreviousPage.Should().Be(1);
            model.NextPage.Should().Be(2);
        }

        [TestMethod]
        public void ShouldCalculateCorrectPagingForResultOfThreePages()
        {
            PagingModel model = new PagingModel((pageSize * 2) + 1, new PagingQuery(1));
            model.PageSize = pageSize;

            model.TotalPages.Should().Be(3);
            model.CurrentPage.Should().Be(1);
            model.FirstPage.Should().Be(2);
            model.LastPage.Should().Be(2);
            model.PreviousPage.Should().Be(1);
            model.NextPage.Should().Be(2);
        }

        [TestMethod]
        public void ShouldCalculateCorrectPagingForResultOfFourPages()
        {
            PagingModel model = new PagingModel((pageSize * 3) + 1, new PagingQuery(1));
            model.PageSize = pageSize;

            model.TotalPages.Should().Be(4);
            model.CurrentPage.Should().Be(1);
            model.FirstPage.Should().Be(2);
            model.LastPage.Should().Be(3);
            model.PreviousPage.Should().Be(1);
            model.NextPage.Should().Be(2);
        }
    }
}
