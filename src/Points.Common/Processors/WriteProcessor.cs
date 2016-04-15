using Points.Common.Factories;
using Points.Model;
using Points.DataAccess.Writers;

namespace Points.Common.Processors
{
    public class WriteProcessor : IWriteProcessor
    {
        private readonly IDataWriter _dataWriter;
        private readonly IObjectValidatorFactory _objectValidatorFactory;
        private readonly IMapFactory _mapFactory;

        public WriteProcessor(IDataWriter dataWriter, IObjectValidatorFactory objectValidatorFactory, IMapFactory mapFactory)
        {
            _dataWriter = dataWriter;
            _objectValidatorFactory = objectValidatorFactory;
            _mapFactory = mapFactory;
        }

        public void AddData<TView>(TView data, string userId) where TView : ModelBase
        {
            // map to RavenObject
            var ravenObj = _mapFactory.MapToRavenObject(data);
            ravenObj.UserId = userId;
            var validator = _objectValidatorFactory.Get(ravenObj.GetType());
            validator?.ValidateAdd(ravenObj);
            _dataWriter.Add(ravenObj);
        }

        public void EditData<TView>(TView data, string userId) where TView : ModelBase
        {
            // map to RavenObject
            var ravenObj = _mapFactory.MapToRavenObject(data);
            ravenObj.UserId = userId;
            var validator = _objectValidatorFactory.Get(ravenObj.GetType());
            validator?.ValidateEdit(ravenObj);
            _dataWriter.Edit(ravenObj);
        }

        public void DeleteData<TView>(TView data, string userId) where TView : ModelBase
        {
            // map to RavenObject
            var ravenObj = _mapFactory.MapToRavenObject(data);
            ravenObj.UserId = userId;
            var validator = _objectValidatorFactory.Get(ravenObj.GetType());
            validator?.ValidateDelete(ravenObj);
            _dataWriter.Delete(ravenObj);
        }
    }
}