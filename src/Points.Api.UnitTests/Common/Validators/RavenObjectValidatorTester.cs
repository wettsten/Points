using System;
using Points.Common.Validators;
using Points.Data;
using Points.DataAccess.Readers;

namespace Points.Api.UnitTests.Common.Validators
{
    public class RavenObjectValidatorTester : RavenObjectValidator, IObjectValidator
    {
        public RavenObjectValidatorTester(IDataReader dataReader) : base(dataReader) { }

        public Type SupportsType => typeof(RavenObject);

        public void ValidateAdd(object data)
        {
            ValidateAdd<RavenObject>(data);
        }

        public void ValidateEdit(object data)
        {
            ValidateEdit<RavenObject>(data);
        }

        public void ValidateDelete(object data)
        {
            ValidateDelete<RavenObject>(data);
        }
    }
}