using System;
using Points.Data;
using Points.DataAccess;

namespace Points.Common.Validators
{
    public class PlanningTaskValidator : RavenObjectValidator, IObjectValidator
    {
        public PlanningTaskValidator(DataReader dataReader) : base(dataReader) { }

        public Type SupportsType => typeof(PlanningTask);

        public void ValidateAdd(object data)
        {
            ValidateAdd<PlanningTask>(data);
        }

        public void ValidateEdit(object data)
        {
            ValidateEdit<PlanningTask>(data);
        }

        public void ValidateDelete(object data)
        {
            ValidateDelete<PlanningTask>(data);
        }
    }
}