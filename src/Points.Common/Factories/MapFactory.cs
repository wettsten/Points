using System;
using System.Linq;
using AutoMapper;
using Points.Data;
using Points.Model;

namespace Points.Common.Factories
{
    public class MapFactory : IMapFactory
    {
        private readonly IMapper _mapper;

        public MapFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public DataBase MapToRavenObject<TView>(TView obj) where TView : ModelBase
        {
            var sourceType = obj.GetType();
            var destinationType = _mapper
                .ConfigurationProvider
                .GetAllTypeMaps()
                .Single(map => map.SourceType == sourceType)
                .DestinationType;
            return (DataBase)_mapper.Map(obj, sourceType, destinationType);
        }

        public ModelBase MapToViewObject<TRaven>(TRaven obj) where TRaven : DataBase
        {
            var sourceType = obj.GetType();
            var destinationType = _mapper
                .ConfigurationProvider
                .GetAllTypeMaps()
                .Single(map => map.SourceType == sourceType)
                .DestinationType;
            return (ModelBase)_mapper.Map(obj, sourceType, destinationType);
        }

        public Type GetDestinationType(Type sourceType)
        {
            return _mapper
                .ConfigurationProvider
                .GetAllTypeMaps()
                .SingleOrDefault(map => map.SourceType == sourceType)?
                .DestinationType;
        }
    }
}