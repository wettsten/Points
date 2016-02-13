using System;
using System.IO;
using System.Linq;
using Points.Data;
using Points.DataAccess;
using Points.DataAccess.Readers;

namespace Points.Common.Validators
{
    public abstract class RavenObjectValidator
    {
        protected readonly DataReader DataReader;

        protected RavenObjectValidator(DataReader dataReader)
        {
            DataReader = dataReader;
        }

        protected void ValidateAdd<T>(object data) where T : RavenObject
        {
            var obj = data as RavenObject;
            var objs = DataReader.GetAll<T>();
            if (objs
                .Where(i => i.Name.Equals(obj.Name, StringComparison.InvariantCultureIgnoreCase))
                .Any(i => i.UserId.Equals(obj.UserId, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new InvalidDataException("This name is already in use");
            }
            if (!(obj is User))
            {
                var user = DataReader.Get<User>(obj.UserId);
                if (user == null)
                {
                    throw new InvalidDataException("User id is invalid");
                }
            }
        }

        protected void ValidateEdit<T>(object data) where T : RavenObject
        {
            var obj = data as RavenObject;
            var objs = DataReader.GetAll<T>();
            if (objs
                .Where(i => i.Name.Equals(obj.Name, StringComparison.InvariantCultureIgnoreCase))
                .Where(i => !i.Id.Equals(obj.Id))
                .Any(i => i.UserId.Equals(obj.UserId, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new InvalidDataException("This name is already in use");
            }
            if (!(obj is User))
            {
                var user = DataReader.Get<User>(obj.UserId);
                if (user == null)
                {
                    throw new InvalidDataException("User id is invalid");
                }
            }
        }

        protected void ValidateDelete<T>(object data) where T : RavenObject
        {
            var obj = data as RavenObject;
            var res = DataReader.Get<T>(obj.Id);
            if (res == null)
            {
                throw new InvalidDataException("Item does not exist or has already been deleted");
            }
            if (!(obj is User))
            {
                var user = DataReader.Get<User>(obj.UserId);
                if (user == null)
                {
                    throw new InvalidDataException("User id is invalid");
                }
            }
        }
    }
}