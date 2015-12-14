using System;
using System.IO;
using Points.Data;
using Points.DataAccess;

namespace Points.Common.Validators
{
    public class TaskValidator : RavenObjectValidator, IObjectValidator
    {
        public TaskValidator(DataReader dataReader) : base(dataReader) { }

        public Type SupportsType => typeof(Task);

        public void ValidateAdd(object data)
        {
            ValidateAdd<Task>(data);
            var obj = data as Task;
            var cat = DataReader.Get<Category>(obj.CategoryId);
            if (cat == null)
            {
                throw new InvalidDataException("Category is invalid");
            }
        }

        public void ValidateEdit(object data)
        {
            ValidateEdit<Task>(data);
            var obj = data as Task;
            var cat = DataReader.Get<Category>(obj.CategoryId);
            if (cat == null)
            {
                throw new InvalidDataException("Category is invalid");
            }
        }

        public void ValidateDelete(object data)
        {
            ValidateDelete<Task>(data);
        }
    }
}