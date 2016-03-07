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
    public class TaskValidatorTests : AutoMoqTestFixture<TaskValidator>
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            var tasks = new List<PlanningTask>
            {
                new PlanningTask { Id = Guido.New(), Name = "task1", UserId = "user1", TaskId = "task1"}
            };
            Mocked<IDataReader>().Setup(r => r.GetAll<PlanningTask>()).Returns(tasks);
            Mocked<IDataReader>().Setup(r => r.GetAll<Task>()).Returns(new List<Task>());
        }

        [SetUp]
        public void TestSetup()
        {
            Mocked<IDataReader>().Setup(r => r.Get<User>(It.IsAny<string>())).Returns(new User());
            Mocked<IDataReader>().Setup(r => r.Get<Task>(It.IsAny<string>())).Returns(new Task());
            Mocked<IDataReader>().Setup(r => r.Get<Category>(It.IsAny<string>())).Returns(new Category());
        }

        [Test]
        public void ValidateAddCategoryIsValid()
        {
            var task = new Task { Id = "task4", Name = "task4", UserId = "user4", CategoryId = "cat4"};
            Assert.DoesNotThrow(() => Subject.ValidateAdd(task));
        }

        [Test]
        public void ValidateAddCategoryInvalid()
        {
            Mocked<IDataReader>().Setup(r => r.Get<Category>(It.IsAny<string>())).Returns(() => null);
            var task = new Task { Id = "task1", Name = "task1", UserId = "user1", CategoryId = "cat1"};
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateAdd(task));
            ex.Message.ShouldBe("Category is invalid");
        }

        [Test]
        public void ValidateEditCategoryIsValid()
        {
            var task = new Task { Id = "task4", Name = "task4", UserId = "user4", CategoryId = "cat4" };
            Assert.DoesNotThrow(() => Subject.ValidateEdit(task));
        }

        [Test]
        public void ValidateEditCategoryInvalid()
        {
            Mocked<IDataReader>().Setup(r => r.Get<Category>(It.IsAny<string>())).Returns(() => null);
            var task = new Task { Id = "task1", Name = "task1", UserId = "user1", CategoryId = "cat1" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateEdit(task));
            ex.Message.ShouldBe("Category is invalid");
        }

        [Test]
        public void ValidateDeleteTaskNotInUse()
        {
            var task = new Task {Id = "task4", Name = "task4", UserId = "user4"};
            Assert.DoesNotThrow(() => Subject.ValidateDelete(task));
        }

        [Test]
        public void ValidateDeleteTaskInUse()
        {
            var task = new Task { Id = "task1", Name = "task1", UserId = "user1" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateDelete(task));
            ex.Message.ShouldBe("Task is currently in use");
        }
    }
}