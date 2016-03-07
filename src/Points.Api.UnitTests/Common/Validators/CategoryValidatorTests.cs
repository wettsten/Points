using System.Collections.Generic;
using System.IO;
using AutoMoq.Helpers;
using Moq;
using NUnit.Framework;
using Points.Common.Validators;
using Points.DataAccess.Readers;
using Points.Data;
using Shouldly;

namespace Points.Api.UnitTests.Common.Validators
{
    [TestFixture]
    public class CategoryValidatorTests : AutoMoqTestFixture<CategoryValidator>
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            var tasks = new List<Task>
            {
                new Task { Id = Guido.New(), Name = "task1", UserId = "user1", CategoryId = "cat1"},
                new Task { Id = Guido.New(), Name = "task2", UserId = "user2", CategoryId = "cat2"},
                new Task { Id = Guido.New(), Name = "task3", UserId = "user3", CategoryId = "cat3"}
            };
            Mocked<IDataReader>().Setup(r => r.GetAll<Task>()).Returns(tasks);
        }

        [SetUp]
        public void TestSetup()
        {
            Mocked<IDataReader>().Setup(r => r.Get<User>(It.IsAny<string>())).Returns(new User());
            Mocked<IDataReader>().Setup(r => r.Get<Category>(It.IsAny<string>())).Returns(new Category());
        }

        [Test]
        public void ValidateDeleteCategoryNotInUse()
        {
            var cat = new Category {Id = "cat4", Name = "cat4", UserId = "user4"};
            Assert.DoesNotThrow(() => Subject.ValidateDelete(cat));
        }

        [Test]
        public void ValidateDeleteCategoryInUse()
        {
            var cat = new Category { Id = "cat1", Name = "cat1", UserId = "user1" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateDelete(cat));
            ex.Message.ShouldBe("Category is currently in use");
        }
    }
}