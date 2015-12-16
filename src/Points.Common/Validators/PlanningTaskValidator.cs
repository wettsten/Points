using System;
using System.IO;
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
            var obj = data as PlanningTask;
            var task = DataReader.Get<Category>(obj.TaskId);
            if (task == null)
            {
                throw new InvalidDataException("Task is invalid");
            }
        }

        public void ValidateEdit(object data)
        {
            ValidateEdit<PlanningTask>(data);
            var obj = data as PlanningTask;
            var task = DataReader.Get<Category>(obj.TaskId);
            if (task == null)
            {
                throw new InvalidDataException("Task is invalid");
            }
        }

        public void ValidateDelete(object data)
        {
            ValidateDelete<PlanningTask>(data);
        }
    }
}