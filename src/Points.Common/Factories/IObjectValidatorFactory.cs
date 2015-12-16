using System;
using Points.Common.Validators;

namespace Points.Common.Factories
{
    public interface IObjectValidatorFactory
    {
        IObjectValidator Get(Type objType);
    }
}