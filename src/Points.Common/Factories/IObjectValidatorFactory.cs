using System;
using System.Collections;
using System.Dynamic;
using Points.Common.Processors;
using Points.Common.Validators;
using Points.Data;

namespace Points.Common.Factories
{
    public interface IObjectValidatorFactory
    {
        IObjectValidator Get(Type objType);
    }
}