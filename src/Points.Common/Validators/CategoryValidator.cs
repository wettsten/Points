using System;
using System.IO;
using System.Linq;
using Points.Data;

namespace Points.Common.Validators
{
    public class CategoryValidator : RavenObjectValidator, IObjectValidator
    {
        public Type SupportsType => typeof(Category);

        public void ValidateAdd(object data)
        {
            Logger.Debug("Validating Add Category");
            ValidateAdd<Category>(data);
            Logger.Debug("Validating Add Category Ok");
        }

        public void ValidateEdit(object data)
        {
            Logger.Debug("Validating Edit Category");
            ValidateEdit<Category>(data);
            Logger.Debug("Validating Edit Category Ok");
        }

        public void ValidateDelete(object data)
        {
            Logger.Debug("Validating Delete Category");
            ValidateDelete<Category>(data);
            var obj = data as Category;
            var tasks = DataReader.GetAll<Task>().Where(i => i.CategoryId.Equals(obj.Id, StringComparison.InvariantCultureIgnoreCase));
            if (tasks.Any())
            {
                Logger.Error("Validating Delete Category error: Category is currently in use");
                throw new InvalidDataException("Category is currently in use");
            }
            Logger.Debug("Validating Delete Category Ok");
        }
    }
}