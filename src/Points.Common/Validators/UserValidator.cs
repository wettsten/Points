using System;
using Points.Data;
using Points.Data.Raven;
using Points.DataAccess;
using Points.DataAccess.Readers;

namespace Points.Common.Validators
{
    public class UserValidator : RavenObjectValidator, IObjectValidator
    {
        public UserValidator(DataReader dataReader) : base(dataReader) { }

        public Type SupportsType => typeof(User);

        public void ValidateAdd(object data)
        {
            ValidateAdd<User>(data);
        }

        public void ValidateEdit(object data)
        {
            ValidateEdit<User>(data);
        }

        public void ValidateDelete(object data)
        {
            ValidateDelete<User>(data);
        }
    }
}