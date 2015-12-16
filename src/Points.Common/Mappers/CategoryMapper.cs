using Points.Data.Raven;
using Points.Data.View;
using RavenCategory = Points.Data.Raven.Category;
using ViewCategory = Points.Data.View.Category;

namespace Points.Common.Mappers
{
    public class CategoryMapper : IObjectMapper<RavenCategory, ViewCategory>
    {
        private readonly IObjectMapper<RavenObject, ViewObject> _baseMapper;

        public CategoryMapper(IObjectMapper<RavenObject, ViewObject> baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public ViewCategory Map(RavenCategory obj)
        {
            var viewCat = (ViewCategory)_baseMapper.Map(obj);
            return viewCat;
        }
    }
}