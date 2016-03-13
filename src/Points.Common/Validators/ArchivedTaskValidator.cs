using System;
using Points.Data;

namespace Points.Common.Validators
{
    public class ArchivedTaskValidator : RavenObjectValidator, IObjectValidator
    {
        public Type SupportsType => typeof(ArchivedTask);

        public void ValidateAdd(object data)
        {
            Logger.DebugFormat("Validating Add ArchivedTask");
            ValidateAdd<ArchivedTask>(data);
            Logger.DebugFormat("Validating Add ArchivedTask Ok");
        }

        public void ValidateEdit(object data)
        {
            Logger.DebugFormat("Validating Edit ArchivedTask");
            ValidateEdit<ArchivedTask>(data);
            Logger.DebugFormat("Validating Edit ArchivedTask Ok");
        }

        public void ValidateDelete(object data)
        {
            Logger.DebugFormat("Validating Delete ArchivedTask");
            ValidateDelete<ArchivedTask>(data);
            Logger.DebugFormat("Validating Delete ArchivedTask Ok");
        }
    }
}