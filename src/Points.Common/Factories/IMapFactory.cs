using System;
using Points.Data;
using Points.Model;

namespace Points.Common.Factories
{
    public interface IMapFactory
    {
        DataBase MapToRavenObject<TView>(TView obj) where TView : ModelBase;
        ModelBase MapToViewObject<TRaven>(TRaven obj) where TRaven : DataBase;

        Type GetDestinationType(Type sourceType);
    }
}