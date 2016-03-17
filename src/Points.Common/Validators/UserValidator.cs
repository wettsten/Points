using System;
using Points.Data;

namespace Points.Common.Validators
{
    public class UserValidator : RavenObjectValidator, IObjectValidator
    {
        public Type SupportsType => typeof(User);

        public void ValidateAdd(object data)
        {
            Logger.Debug("Validating Add User");
            ValidateAdd<User>(data);
            Logger.Debug("Validating Add User Ok");
        }

        public void ValidateEdit(object data)
        {
            Logger.Debug("Validating Edit User");
            ValidateEdit<User>(data);
            Logger.Debug("Validating Edit User Ok");
        }

        public void ValidateDelete(object data)
        {
            Logger.Debug("Validating Delete User");
            ValidateDelete<User>(data);
            Logger.Debug("Validating Delete User Ok");
        }
    }
}