using Points.Data.Raven;
using Points.Data.View;
using RavenUser = Points.Data.Raven.User;
using ViewUser = Points.Data.View.User;

namespace Points.Common.Mappers
{
    public class UserMapper : IObjectMapper<RavenUser, ViewUser>
    {
        private readonly IObjectMapper<RavenObject, ViewObject> _baseMapper;

        public UserMapper(IObjectMapper<RavenObject, ViewObject> baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public ViewUser Map(RavenUser obj)
        {
            var viewUser = new ViewUser();
            viewUser.Copy(_baseMapper.Map(obj));
            viewUser.Email = obj.Email;
            viewUser.NotifyWeekStarting = obj.NotifyWeekStarting;
            viewUser.NotifyWeekEnding = obj.NotifyWeekEnding;
            viewUser.WeekStartDay = obj.WeekStartDay;
            viewUser.WeekStartHour = obj.WeekStartHour;
            return viewUser;
        }
    }
}