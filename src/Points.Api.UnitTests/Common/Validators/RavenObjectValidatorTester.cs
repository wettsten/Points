using System;
using Points.Common.Validators;
using Points.Data;
using Points.DataAccess.Readers;

namespace Points.Api.UnitTests.Common.Validators
{
    public class RavenObjectValidatorTester : RavenObjectValidator, IObjectValidator
    {
        public RavenObjectValidatorTester(IDataReader dataReader) : base(dataReader) { }

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