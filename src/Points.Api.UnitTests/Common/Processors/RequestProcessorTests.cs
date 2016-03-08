using System.Collections.Generic;
using System.Linq;
using AutoMoq.Helpers;
using Moq;
using NUnit.Framework;
using Points.Common.Factories;
using Points.Common.Processors;
using Points.DataAccess.Readers;
using Shouldly;

namespace Points.Api.UnitTests.Common.Processors
{
    [TestFixture]
    public class RequestProcessorTests : AutoMoqTestFixture<RequestProcessor>
    {
        private readonly string _userId = Guido.New();
        private List<Data.RavenObject> _dataObjects;

        [TestFixtureSetUp]
        public void Setup()
        {
            _dataObjects = new List<Data.RavenObject>
            {
                new Data.RavenObject { Id = Guido.New(), Name = "name1", UserId = _userId },
                new Data.RavenObject { Id = Guido.New(), Name = "name2", UserId = Guido.New() },
                new Data.RavenObject { Id = Guido.New(), Name = "name3", UserId = _userId }
            };
            
            Mocked<IMapFactory>().Setup(r => r.GetDestinationType(typeof(Model.ViewObject))).Returns(typeof(Data.RavenObject));
            Mocked<IDataReader>().Setup(r => r.GetAll(typeof (Data.RavenObject))).Returns(_dataObjects);
            Mocked<IMapFactory>().Setup(r => r.MapToViewObject(It.IsAny<Data.RavenObject>())).Returns(new Model.ViewObject());
        }

        [Test]
        public void GetListForUserWithOwnedObjectsFiltersByUserIdReturnsTwo()
        {
            var results = Subject.GetListForUser<Model.ViewObject>(_userId);
            results.Count.ShouldBe(2);
        }

        [Test]
        public void GetListForUserWithoutOwnedObjectsFiltersByUserIdReturnsEmptyList()
        {
            var results = Subject.GetListForUser<Model.ViewObject>(Guido.New());
            results.Count.ShouldBe(0);
        }

        [Test]
        public void GetEnumsValidReturnResults([Values("DurationType","DurationUnit","FrequencyType","FrequencyUnit")]string type)
        {
            var result = Subject.GetEnums(type);
            result.Count().ShouldBeGreaterThan(0);
        }

        [Test]
        public void GetEnumsInvalidReturnEmptyList()
        {
            var result = Subject.GetEnums("");
            result.Count().ShouldBe(0);
        }

        //[Test]
        //public void GetPlanningTotals()
        //{
        //    var totals = Subject.GetPlanningTotals(_userId);
        //}
    }
}