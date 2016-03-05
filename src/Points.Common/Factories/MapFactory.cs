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

        public RavenObject MapToRavenObject<TView>(TView obj) where TView : ViewObject
        {
            var sourceType = obj.GetType();
            var destinationType = _mapper
                .ConfigurationProvider
                .GetAllTypeMaps()
                .Single(map => map.SourceType == sourceType)
                .DestinationType;
            return (RavenObject)_mapper.Map(obj, sourceType, destinationType);
        }

        public ViewObject MapToViewObject<TRaven>(TRaven obj) where TRaven : RavenObject
        {
            var sourceType = obj.GetType();
            var destinationType = _mapper
                .ConfigurationProvider
                .GetAllTypeMaps()
                .Single(map => map.SourceType == sourceType)
                .DestinationType;
            return (ViewObject)_mapper.Map(obj, sourceType, destinationType);
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