using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.EntityTests
{
    [TestClass]
    public class BusinessTests : BaseEntityTest
    {
        [TestMethod]
        public async Task CanCreateReadAndDeleteBusiness()
        {
            // Use a separate context for saving vs retrieving to prevent any caching

            State state = new State("BS", "BusinessState");
            using (TestSqlStorageContext<State> storageContext = new TestSqlStorageContext<State>())
            {
                storageContext.Add(state);
                await storageContext.SaveChangesAsync();
            }

            Business business = new Business()
            {
                State = state.Abbreviation
            };

            using (TestSqlStorageContext<Business> storageContext = new TestSqlStorageContext<Business>())
            {
                storageContext.FillWithTestData(business, "State");
                storageContext.Add(business);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Business> storageContext = new TestSqlStorageContext<Business>())
            {
                Business result = await storageContext.Entities.FindByIDAsync(business.ID);
                result.Should().NotBeNull();

                result.Name.Should().Be(business.Name);
                result.Email.Should().Be(business.Email);
                result.Address.Should().Be(business.Address);
                result.City.Should().Be(business.City);
                result.Zip.Should().Be(business.Zip);
                result.State.Should().Be(business.State);
                result.RescueGroupsID.Should().Be(business.RescueGroupsID);

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.Entities.MatchingID(business.ID).SingleOrDefault().Should().BeNull();
            }
        }

        [TestMethod]
        public async Task CanCreateReadAndDeleteBusinessGroup()
        {
            Group group1 = new Group("business1");
            Group group2 = new Group("business2");

            Business business = new Business();
            business.AddGroup(group1);
            business.AddGroup(group2);

            using (TestSqlStorageContext<Business> storageContext = new TestSqlStorageContext<Business>())
            {
                storageContext.Add(business);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Business> storageContext = new TestSqlStorageContext<Business>())
            {
                Business result = await storageContext.Entities.FindByIDAsync(business.ID);
                result.Should().NotBeNull();

                result.Groups.Should().HaveCount(2);

                BusinessGroup businessGroup1 = result.Groups.SingleOrDefault(x => x.GroupID == group1.ID);
                businessGroup1.BusinessID.Should().Be(business.ID);
                businessGroup1.Business.Should().NotBeNull();
                businessGroup1.Business.ID.Should().Be(business.ID);
                businessGroup1.Group.Should().NotBeNull();
                businessGroup1.Group.ID.Should().Be(group1.ID);

                BusinessGroup businessGroup2 = result.Groups.SingleOrDefault(x => x.GroupID == group2.ID);
                businessGroup2.BusinessID.Should().Be(business.ID);
                businessGroup2.Business.Should().NotBeNull();
                businessGroup2.Business.ID.Should().Be(business.ID);
                businessGroup2.Group.Should().NotBeNull();
                businessGroup2.Group.ID.Should().Be(group2.ID);

                // Can group be removed from business?
                result.Groups.Remove(businessGroup1);
                await storageContext.SaveChangesAsync();

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();

                storageContext.Entities.MatchingID(business.ID).SingleOrDefault().Should().BeNull();
            }

            using (TestSqlStorageContext<Group> storageContext = new TestSqlStorageContext<Group>())
            {
                Group result1 = await storageContext.Entities.SingleOrDefaultAsync(x => x.ID == group1.ID);
                result1.Should().NotBeNull("group should still exist after business linked to group is deleted");

            }
        }

        [TestMethod]
        public async Task CanReadWriteAndDeleteBusinessPhone()
        {
            PhoneType phoneType = new PhoneType("bstest");

            using (TestSqlStorageContext<PhoneType> storageContext = new TestSqlStorageContext<PhoneType>())
            {
                storageContext.Add(phoneType);
                await storageContext.SaveChangesAsync();
            }

            Business business = new Business();
            BusinessPhone businessPhone1 = business.AddPhoneNumber("1234567890", "123456", phoneType);
            BusinessPhone businessPhone2 = business.AddPhoneNumber("9876543210", null, phoneType);

            using (TestSqlStorageContext<Business> storageContext = new TestSqlStorageContext<Business>())
            {
                storageContext.Add(business);
                await storageContext.SaveChangesAsync();
            }

            using (TestSqlStorageContext<Business> storageContext = new TestSqlStorageContext<Business>())
            {
                Business result = await storageContext.Entities.FindByIDAsync(business.ID);
                result.Should().NotBeNull();

                result.PhoneNumbers.Should().HaveCount(2);

                BusinessPhone resultPhone = result.PhoneNumbers.Single(x => x.PhoneNumber == businessPhone1.PhoneNumber);

                resultPhone.BusinessID.Should().Be(business.ID);
                resultPhone.Business.Should().NotBeNull();
                resultPhone.Business.ID.Should().Be(business.ID);

                resultPhone.PhoneTypeID.Should().Be(phoneType.ID);
                resultPhone.PhoneType.Should().NotBeNull();
                resultPhone.PhoneType.ID.Should().Be(phoneType.ID);

                resultPhone.PhoneExtension.Should().Be(businessPhone1.PhoneExtension);

                // Can phone be removed from business?
                result.PhoneNumbers.Remove(resultPhone);
                await storageContext.SaveChangesAsync();

                storageContext.Delete(result);
                await storageContext.SaveChangesAsync();
            }
        }
    }
}
