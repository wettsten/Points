using Points.Data.Raven;
using Points.Data.View;

namespace Points.Common.Mappers
{
    public interface IObjectMapper<in TIn, out TOut> where TIn : RavenObject where TOut : ViewObject
    {
        TOut Map(TIn obj);
    }
}