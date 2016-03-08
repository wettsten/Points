using System;
using AutoMapper;
using AutoMoq.Helpers;
using NUnit.Framework;
using Points.Common.AutoMapper;
using Points.Common.Factories;
using Shouldly;

namespace Points.Api.UnitTests.Common.Factories
{
    [TestFixture]
    public class MapFactoryTests : AutoMoqTestFixture<MappingProfile>
    {
        private MapFactory _sut;

        [OneTimeSetUp]
        [TestFixtureSetUp]
        public void Setup()
        {
            var mapper = new MapperConfiguration(m => m.AddProfile(Subject)).CreateMapper();
            _sut = new MapFactory(mapper);
        }

        [Test]
        public void TestMapToRavenObjectDoesNotThrow()
        {
            Assert.DoesNotThrow(() => _sut.MapToRavenObject(new Model.ModelBase()));
        }

        [Test]
        public void TestMapToViewObjectDoesNotThrow()
        {
            Assert.DoesNotThrow(() => _sut.MapToViewObject(new Data.DataBase()));
        }

        [Test]
        [TestCase(typeof(Data.DataBase), typeof(Model.ModelBase))]
        [TestCase(typeof(Data.Category), typeof(Model.Category))]
        [TestCase(typeof(Data.Task), typeof(Model.Task))]
        [TestCase(typeof(Data.PlanningTask), typeof(Model.PlanningTask))]
        [TestCase(typeof(Data.ActiveTask), typeof(Model.ActiveTask))]
        [TestCase(typeof(Data.ArchivedTask), typeof(Model.ArchivedTask))]
        [TestCase(typeof(Data.Job), typeof(Model.Job))]
        [TestCase(typeof(Data.User), typeof(Model.User))]
        [TestCase(typeof(Data.Duration), typeof(Model.Duration))]
        [TestCase(typeof(Data.Frequency), typeof(Model.Frequency))]
        [TestCase(typeof(Model.ModelBase), typeof(Data.DataBase))]
        [TestCase(typeof(Model.Category), typeof(Data.Category))]
        [TestCase(typeof(Model.Task), typeof(Data.Task))]
        [TestCase(typeof(Model.PlanningTask), typeof(Data.PlanningTask))]
        [TestCase(typeof(Model.ActiveTask), typeof(Data.ActiveTask))]
        [TestCase(typeof(Model.ArchivedTask), typeof(Data.ArchivedTask))]
        [TestCase(typeof(Model.Job), typeof(Data.Job))]
        [TestCase(typeof(Model.User), typeof(Data.User))]
        [TestCase(typeof(Model.Duration), typeof(Data.Duration))]
        [TestCase(typeof(Model.Frequency), typeof(Data.Frequency))]
        public void TestGetDestinationTypeInclusions(Type inputType, Type outputType)
        {
            var dest = _sut.GetDestinationType(inputType);
            dest.ShouldBe(outputType);
        }

        [Test]
        public void TestGetDestinationTypeExclusionReturnsNull()
        {
            var dest = _sut.GetDestinationType(typeof(Model.SimpleInt));
            dest.ShouldBeNull();
        }
    }
}