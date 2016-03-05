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
    public class PlanningTaskValidatorTests : AutoMoqTestFixture<PlanningTaskValidator>
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            Mocked<IDataReader>().Setup(r => r.GetAll<PlanningTask>()).Returns(new List<PlanningTask>());
        }

        [SetUp]
        public void TestSetup()
        {
            Mocked<IDataReader>().Setup(r => r.Get<User>(It.IsAny<string>())).Returns(new User());
            Mocked<IDataReader>().Setup(r => r.Get<PlanningTask>(It.IsAny<string>())).Returns(new PlanningTask());
            Mocked<IDataReader>().Setup(r => r.Get<Task>(It.IsAny<string>())).Returns(new Task());
        }

        [Test]
        public void ValidateAddTaskIsValid()
        {
            var task = new PlanningTask { Id = "ptask4", Name = "ptask4", UserId = "user4", TaskId = "task4" };
            Assert.DoesNotThrow(() => Subject.ValidateAdd(task));
        }

        [Test]
        public void ValidateAddTaskInvalid()
        {
            Mocked<IDataReader>().Setup(r => r.Get<Task>(It.IsAny<string>())).Returns(() => null);
            var task = new PlanningTask { Id = "ptask1", Name = "ptask1", UserId = "user1", TaskId = "task1" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateAdd(task));
            ex.Message.ShouldBe("Task is invalid");
        }

        [Test]
        public void ValidateEditTaskIsValid()
        {
            var task = new PlanningTask { Id = "ptask4", Name = "ptask4", UserId = "user4", TaskId = "task4" };
            Assert.DoesNotThrow(() => Subject.ValidateEdit(task));
        }

        [Test]
        public void ValidateEditTaskInvalid()
        {
            Mocked<IDataReader>().Setup(r => r.Get<Task>(It.IsAny<string>())).Returns(() => null);
            var task = new PlanningTask { Id = "ptask1", Name = "ptask1", UserId = "user1", TaskId = "task1" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateEdit(task));
            ex.Message.ShouldBe("Task is invalid");
        }
    }
}