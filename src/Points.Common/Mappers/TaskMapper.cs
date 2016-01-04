using Points.Data.Raven;
using Points.Data.View;
using Points.DataAccess;
using Points.DataAccess.Readers;
using RavenCategory = Points.Data.Raven.Category;
using ViewCategory = Points.Data.View.Category;
using RavenTask = Points.Data.Raven.Task;
using ViewTask = Points.Data.View.Task;

namespace Points.Common.Mappers
{
    public class TaskMapper : IObjectMapper<RavenTask, ViewTask>
    {
        private readonly IDataReader _dataReader;
        private readonly IObjectMapper<RavenCategory, ViewCategory> _catMapper;
        private readonly IObjectMapper<RavenObject, ViewObject> _baseMapper;

        public TaskMapper(IDataReader dataReader, IObjectMapper<RavenCategory, ViewCategory> catMapper, IObjectMapper<RavenObject, ViewObject> baseMapper)
        {
            _dataReader = dataReader;
            _catMapper = catMapper;
            _baseMapper = baseMapper;
        }

        public ViewTask Map(RavenTask obj)
        {
            var viewTask = new ViewTask();
            viewTask.Copy(_baseMapper.Map(obj));
            var cat = _dataReader.Get<RavenCategory>(obj.CategoryId);
            viewTask.Category = _catMapper.Map(cat);
            return viewTask;
        }
    }
}