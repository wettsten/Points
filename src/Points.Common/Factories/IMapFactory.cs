using System;
using Points.Data;
using Points.Model;

namespace Points.Common.Factories
{
    public interface IMapFactory
    {
        RavenObject MapToRavenObject<TView>(TView obj) where TView : ViewObject;
        ViewObject MapToViewObject<TRaven>(TRaven obj) where TRaven : RavenObject;

        Type GetDestinationType(Type sourceType);
    }
}