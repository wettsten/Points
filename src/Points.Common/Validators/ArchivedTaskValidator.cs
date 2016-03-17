using System;
using Points.Data;

namespace Points.Common.Validators
{
    public class ArchivedTaskValidator : RavenObjectValidator, IObjectValidator
    {
        public Type SupportsType => typeof(ArchivedTask);

        public void ValidateAdd(object data)
        {
            Logger.Debug("Validating Add ArchivedTask");
            ValidateAdd<ArchivedTask>(data);
            Logger.Debug("Validating Add ArchivedTask Ok");
        }

        public void ValidateEdit(object data)
        {
            Logger.Debug("Validating Edit ArchivedTask");
            ValidateEdit<ArchivedTask>(data);
            Logger.Debug("Validating Edit ArchivedTask Ok");
        }

        public void ValidateDelete(object data)
        {
            Logger.Debug("Validating Delete ArchivedTask");
            ValidateDelete<ArchivedTask>(data);
            Logger.Debug("Validating Delete ArchivedTask Ok");
        }
    }
}