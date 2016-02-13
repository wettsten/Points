using System;
using Points.Data;
using Points.DataAccess;
using Points.DataAccess.Readers;

namespace Points.Common.Validators
{
    public class ArchivedTaskValidator : RavenObjectValidator, IObjectValidator
    {
        public ArchivedTaskValidator(DataReader dataReader) : base(dataReader) { }

        public Type SupportsType => typeof(ArchivedTask);

        public void ValidateAdd(object data)
        {
            ValidateAdd<ArchivedTask>(data);
        }

        public void ValidateEdit(object data)
        {
            ValidateEdit<ArchivedTask>(data);
        }

        public void ValidateDelete(object data)
        {
            ValidateDelete<ArchivedTask>(data);
        }
    }
}