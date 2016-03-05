using System.Collections.Generic;
using System.IO;
using AutoMoq.Helpers;
using Moq;
using NUnit.Framework;
using Points.DataAccess.Readers;
using Points.Data;
using Shouldly;

namespace Points.Api.UnitTests.Common.Validators
{
    [TestFixture]
    public class RavenObjectValidatorTests : AutoMoqTestFixture<RavenObjectValidatorTester>
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            var ravenObjects = new List<RavenObject>
            {
                new RavenObject { Id = Guido.New(), Name = "object1", UserId = "user1" },
                new RavenObject { Id = Guido.New(), Name = "object2", UserId = "user2" },
                new RavenObject { Id = Guido.New(), Name = "object3", UserId = "user3" }
            };
            Mocked<IDataReader>().Setup(r => r.GetAll<RavenObject>()).Returns(ravenObjects);
        }

        [SetUp]
        public void TestSetup()
        {
            Mocked<IDataReader>().Setup(r => r.Get<User>(It.IsAny<string>())).Returns(new User());
            Mocked<IDataReader>().Setup(r => r.Get<RavenObject>(It.IsAny<string>())).Returns(new RavenObject());
        }

        [Test]
        [TestCase("object4", "user4")]
        [TestCase("object3", "user4")]
        [TestCase("object4", "user3")]
        [TestCase("object2", "user3")]
        public void ValidateAddOk(string name, string userId)
        {
            var ravenObj = new RavenObject {Name = name, UserId = userId};
            Assert.DoesNotThrow(() => Subject.ValidateAdd(ravenObj));
        }

        [Test]
        public void ValidateAddNameAlreadyExists()
        {
            var ravenObj = new RavenObject { Name = "object1", UserId = "user1" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateAdd(ravenObj));
            ex.Message.ShouldBe("This name is already in use");
        }

        [Test]
        public void ValidateAddUserIdIsInvalid()
        {
            Mocked<IDataReader>().Setup(r => r.Get<User>(It.IsAny<string>())).Returns(() => null);
            var ravenObj = new RavenObject { Name = "object4", UserId = "user4" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateAdd(ravenObj));
            ex.Message.ShouldBe("User id is invalid");
        }

        [Test]
        [TestCase("object4", "user4")]
        [TestCase("object3", "user4")]
        [TestCase("object4", "user3")]
        [TestCase("object2", "user3")]
        public void ValidateEditOk(string name, string userId)
        {
            var ravenObj = new RavenObject { Name = name, UserId = userId };
            Assert.DoesNotThrow(() => Subject.ValidateEdit(ravenObj));
        }

        [Test]
        public void ValidateEditNameAlreadyExists()
        {
            var ravenObj = new RavenObject { Name = "object1", UserId = "user1" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateEdit(ravenObj));
            ex.Message.ShouldBe("This name is already in use");
        }

        [Test]
        public void ValidateEditUserIdIsInvalid()
        {
            Mocked<IDataReader>().Setup(r => r.Get<User>(It.IsAny<string>())).Returns(() => null);
            var ravenObj = new RavenObject { Name = "object4", UserId = "user4" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateEdit(ravenObj));
            ex.Message.ShouldBe("User id is invalid");
        }

        [Test]
        public void ValidateDeleteOk()
        {
            var ravenObj = new RavenObject { Name = "object1", UserId = "user1" };
            Assert.DoesNotThrow(() => Subject.ValidateDelete(ravenObj));
        }

        [Test]
        public void ValidateDeleteDoesNotExist()
        {
            Mocked<IDataReader>().Setup(r => r.Get<RavenObject>(It.IsAny<string>())).Returns(() => null);
            var ravenObj = new RavenObject { Name = "object1", UserId = "user1" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateDelete(ravenObj));
            ex.Message.ShouldBe("Item does not exist or has already been deleted");
        }

        [Test]
        public void ValidateDeleteUserIdIsInvalid()
        {
            Mocked<IDataReader>().Setup(r => r.Get<User>(It.IsAny<string>())).Returns(() => null);
            var ravenObj = new RavenObject { Name = "object4", UserId = "user4" };
            var ex = Assert.Throws<InvalidDataException>(() => Subject.ValidateDelete(ravenObj));
            ex.Message.ShouldBe("User id is invalid");
        }
    }
}