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
    public class ReadProcessorTests : AutoMoqTestFixture<ReadProcessor>
    {
        private readonly string _userId = Guido.New();
        private List<Data.DataBase> _dataObjects;

        [TestFixtureSetUp]
        public void Setup()
        {
            _dataObjects = new List<Data.DataBase>
            {
                new Data.DataBase { Id = Guido.New(), Name = "name1", UserId = _userId },
                new Data.DataBase { Id = Guido.New(), Name = "name2", UserId = Guido.New() },
                new Data.DataBase { Id = Guido.New(), Name = "name3", UserId = _userId }
            };
            
            Mocked<IMapFactory>().Setup(r => r.GetDestinationType(typeof(Model.ModelBase))).Returns(typeof(Data.DataBase));
            Mocked<IDataReader>().Setup(r => r.GetAll(typeof (Data.DataBase))).Returns(_dataObjects);
            Mocked<IMapFactory>().Setup(r => r.MapToViewObject(It.IsAny<Data.DataBase>())).Returns(new Model.ModelBase());
        }

        [Test]
        public void GetListForUserWithOwnedObjectsFiltersByUserIdReturnsTwo()
        {
            var results = Subject.GetListForUser<Model.ModelBase>(_userId);
            results.Count().ShouldBe(2);
        }

        [Test]
        public void GetListForUserWithoutOwnedObjectsFiltersByUserIdReturnsEmptyList()
        {
            var results = Subject.GetListForUser<Model.ModelBase>(Guido.New());
            results.Count().ShouldBe(0);
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
    }
}