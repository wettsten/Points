using System;
using System.IO;
using Points.Data;

namespace Points.Common.Validators
{
    public class PlanningTaskValidator : RavenObjectValidator, IObjectValidator
    {
        public Type SupportsType => typeof(PlanningTask);

        public void ValidateAdd(object data)
        {
            Logger.Debug("Validating Add PlanningTask");
            ValidateAdd<PlanningTask>(data);
            var obj = data as PlanningTask;
            var task = DataReader.Get<Task>(obj.TaskId);
            if (task == null)
            {
                Logger.Error("Validating Add PlanningTask error: Task does not exist");
                throw new InvalidDataException("Task does not exist");
            }
            Logger.Debug("Validating Add PlanningTask Ok");
        }

        public void ValidateEdit(object data)
        {
            Logger.Debug("Validating Edit PlanningTask");
            ValidateEdit<PlanningTask>(data);
            var obj = data as PlanningTask;
            var task = DataReader.Get<Task>(obj.TaskId);
            if (task == null)
            {
                Logger.Error("Validating Edit PlanningTask error: Task does not exist");
                throw new InvalidDataException("Task does not exist");
            }
            Logger.Debug("Validating Edit PlanningTask Ok");
        }

        public void ValidateDelete(object data)
        {
            Logger.Debug("Validating Delete PlanningTask");
            ValidateDelete<PlanningTask>(data);
            Logger.Debug("Validating Delete PlanningTask Ok");
        }
    }
}