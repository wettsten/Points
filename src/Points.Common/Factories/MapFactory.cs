using System;
using System.Linq;
using AutoMapper;
using log4net;
using Points.Data;
using Points.Model;

namespace Points.Common.Factories
{
    public class MapFactory : IMapFactory
    {
        private readonly IMapper _mapper;
        private readonly ILog _logger = LogManager.GetLogger("Common Mapping");

        public MapFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public DataBase MapToRavenObject<TView>(TView obj) where TView : ModelBase
        {
            var sourceType = obj.GetType();
            var destinationType = GetDestinationType(sourceType);
            _logger.DebugFormat("Mapping Model to Data source type {0} to destination type {1}", sourceType.FullName, destinationType.FullName);
            return (DataBase)_mapper.Map(obj, sourceType, destinationType);
        }

        public ModelBase MapToViewObject<TRaven>(TRaven obj) where TRaven : DataBase
        {
            var sourceType = obj.GetType();
            var destinationType = GetDestinationType(sourceType);
            _logger.DebugFormat("Mapping Data to Model source type {0} to destination type {1}", sourceType.FullName, destinationType.FullName);
            return (ModelBase)_mapper.Map(obj, sourceType, destinationType);
        }

        public Type GetDestinationType(Type sourceType)
        {
            return _mapper
                .ConfigurationProvider
                .GetAllTypeMaps()
                .Where(map => map.SourceType.Name.Equals(map.DestinationType.Name) || (map.SourceType.Name.EndsWith("Base") && map.DestinationType.Name.EndsWith("Base")))
                .SingleOrDefault(map => map.SourceType == sourceType)?
                .DestinationType;
        }
    }
}