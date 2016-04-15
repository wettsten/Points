using System;
using Points.Data;

namespace Points.Common.Validators
{
    public class ActiveTaskValidator : RavenObjectValidator, IObjectValidator
    {
        public Type SupportsType => typeof(ActiveTask);

        public void ValidateAdd(object data)
        {
            Logger.Debug("Validating Add ActiveTask");
            ValidateAdd<ActiveTask>(data);
            Logger.Debug("Validating Add ActiveTask Ok");
        }

        public void ValidateEdit(object data)
        {
            Logger.Debug("Validating Edit ActiveTask");
            ValidateEdit<ActiveTask>(data);
            Logger.Debug("Validating Edit ActiveTask Ok");
        }

        public void ValidateDelete(object data)
        {
            Logger.Debug("Validating Delete ActiveTask");
            ValidateDelete<ActiveTask>(data);
            Logger.Debug("Validating Delete ActiveTask Ok");
        }
    }
}