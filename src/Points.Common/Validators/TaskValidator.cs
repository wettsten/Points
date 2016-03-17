using System;
using System.IO;
using System.Linq;
using Points.Data;

namespace Points.Common.Validators
{
    public class TaskValidator : RavenObjectValidator, IObjectValidator
    {
        public Type SupportsType => typeof(Task);

        public void ValidateAdd(object data)
        {
            Logger.Debug("Validating Add Task");
            ValidateAdd<Task>(data);
            var obj = data as Task;
            var cat = DataReader.Get<Category>(obj.CategoryId);
            if (cat == null)
            {
                Logger.Error("Validating Add Task error: Category does not exist");
                throw new InvalidDataException("Category does not exist");
            }
            Logger.Debug("Validating Add Task Ok");
        }

        public void ValidateEdit(object data)
        {
            Logger.Debug("Validating Edit Task");
            ValidateEdit<Task>(data);
            var obj = data as Task;
            var cat = DataReader.Get<Category>(obj.CategoryId);
            if (cat == null)
            {
                Logger.Error("Validating Edit Task error: Category does not exist");
                throw new InvalidDataException("Category does not exist");
            }
            Logger.Debug("Validating Edit Task Ok");
        }

        public void ValidateDelete(object data)
        {
            Logger.Debug("Validating Delete Task");
            ValidateDelete<Task>(data);
            var obj = data as Task;
            var tasks = DataReader.GetAll<PlanningTask>().Where(i => i.TaskId.Equals(obj.Id, StringComparison.InvariantCultureIgnoreCase));
            if (tasks.Any())
            {
                Logger.Error("Validating Delete Task error: Task is currently in use");
                throw new InvalidDataException("Task is currently in use");
            }
            Logger.Debug("Validating Delete Task Ok");
        }
    }
}