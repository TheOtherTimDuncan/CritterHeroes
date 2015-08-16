using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using FluentAssertions;

namespace CH.Test.AdminDataMapperTests
{
    public class BaseDataMapperTest : BaseTest
    {
        public void ValidateDataItem(DataItem dataItem, string expectedValue, bool isValid)
        {
            dataItem.Value.Should().Be(expectedValue);
            dataItem.IsValid.Should().Be(isValid);
        }
    }
}
