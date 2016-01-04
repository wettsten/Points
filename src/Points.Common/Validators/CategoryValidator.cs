using System;
using System.IO;
using System.Linq;
using Points.Data;
using Points.Data.Raven;
using Points.DataAccess;
using Points.DataAccess.Readers;

namespace Points.Common.Validators
{
    public class CategoryValidator : RavenObjectValidator, IObjectValidator
    {
        public CategoryValidator(DataReader dataReader) : base(dataReader) { }

        public Type SupportsType => typeof(Category);

        public void ValidateAdd(object data)
        {
            ValidateAdd<Category>(data);
        }

        public void ValidateEdit(object data)
        {
            ValidateEdit<Category>(data);
        }

        public void ValidateDelete(object data)
        {
            ValidateDelete<Category>(data);
            var obj = data as Category;
            var tasks = DataReader.GetAll<Task>().Where(i => i.CategoryId.Equals(obj.Id, StringComparison.InvariantCultureIgnoreCase));
            if (tasks.Any())
            {
                throw new InvalidDataException("Category is currently in use");
            }
        }
    }
}