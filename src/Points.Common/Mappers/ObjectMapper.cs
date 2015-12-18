using Points.Data.Raven;
using Points.Data.View;

namespace Points.Common.Mappers
{
    public class ObjectMapper : IObjectMapper<RavenObject, ViewObject>
    {
        public ViewObject Map(RavenObject obj)
        {
            return new ViewObject
            {
                Id = obj.Id,
                Name = obj.Name,
                IsDeleted = obj.IsDeleted,
                UserId = obj.UserId
            };
        }
    }
}