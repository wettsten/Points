using System;
using Points.Data;
using Points.DataAccess;
using Points.DataAccess.Readers;

namespace Points.Common.Validators
{
    public class ActiveTaskValidator : RavenObjectValidator, IObjectValidator
    {
        public ActiveTaskValidator(IDataReader dataReader) : base(dataReader) { }

        public Type SupportsType => typeof(ActiveTask);

        public void ValidateAdd(object data)
        {
            ValidateAdd<ActiveTask>(data);
        }

        public void ValidateEdit(object data)
        {
            ValidateEdit<ActiveTask>(data);
        }

        public void ValidateDelete(object data)
        {
            ValidateDelete<ActiveTask>(data);
        }
    }
}