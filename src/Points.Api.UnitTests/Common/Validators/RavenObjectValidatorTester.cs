using System;
using Points.Common.Validators;
using Points.Data;

namespace Points.Api.UnitTests.Common.Validators
{
    public class RavenObjectValidatorTester : RavenObjectValidator, IObjectValidator
    {
        public Type SupportsType => typeof(DataBase);

        public void ValidateAdd(object data)
        {
            ValidateAdd<DataBase>(data);
        }

        public void ValidateEdit(object data)
        {
            ValidateEdit<DataBase>(data);
        }

        public void ValidateDelete(object data)
        {
            ValidateDelete<DataBase>(data);
        }
    }
}