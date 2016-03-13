using System;
using Points.Data;

namespace Points.Common.Validators
{
    public class ActiveTaskValidator : RavenObjectValidator, IObjectValidator
    {
        public Type SupportsType => typeof(ActiveTask);

        public void ValidateAdd(object data)
        {
            Logger.DebugFormat("Validating Add ActiveTask");
            ValidateAdd<ActiveTask>(data);
            Logger.DebugFormat("Validating Add ActiveTask Ok");
        }

        public void ValidateEdit(object data)
        {
            Logger.DebugFormat("Validating Edit ActiveTask");
            ValidateEdit<ActiveTask>(data);
            Logger.DebugFormat("Validating Edit ActiveTask Ok");
        }

        public void ValidateDelete(object data)
        {
            Logger.DebugFormat("Validating Delete ActiveTask");
            ValidateDelete<ActiveTask>(data);
            Logger.DebugFormat("Validating Delete ActiveTask Ok");
        }
    }
}