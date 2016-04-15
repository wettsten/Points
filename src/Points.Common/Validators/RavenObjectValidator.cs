using System;
using System.IO;
using System.Linq;
using NLog;
using Points.Data;
using Points.DataAccess.Readers;
using StructureMap.Attributes;

namespace Points.Common.Validators
{
    public abstract class RavenObjectValidator
    {
        [SetterProperty]
        public IDataReader DataReader { get; set; }
        public ILogger Logger => LogManager.GetLogger("Common Validation");

        protected void ValidateAdd<T>(object data) where T : DataBase
        {
            Logger.Debug("Validating Add base object");
            var obj = data as DataBase;
            var objs = DataReader.GetAll<T>();
            if (objs
                .Where(i => i.Name.Equals(obj.Name, StringComparison.InvariantCultureIgnoreCase))
                .Any(i => i.UserId.Equals(obj.UserId, StringComparison.InvariantCultureIgnoreCase)))
            {
                Logger.Debug("Validating Add base object error: This name is already in use");
                throw new InvalidDataException("This name is already in use");
            }
            if (!(obj is User))
            {
                var user = DataReader.Get<User>(obj.UserId);
                if (user == null)
                {
                    Logger.Debug("Validating Add base object error: User id does not exist");
                    throw new InvalidDataException("User id does not exist");
                }
            }
            Logger.Debug("Validating Add base object Ok");
        }

        protected void ValidateEdit<T>(object data) where T : DataBase
        {
            Logger.Debug("Validating Edit base object");
            var obj = data as DataBase;
            var objs = DataReader.GetAll<T>();
            if (objs
                .Where(i => i.Name.Equals(obj.Name, StringComparison.InvariantCultureIgnoreCase))
                .Where(i => !i.Id.Equals(obj.Id))
                .Any(i => i.UserId.Equals(obj.UserId, StringComparison.InvariantCultureIgnoreCase)))
            {
                Logger.Debug("Validating Edit base object error: This name is already in use");
                throw new InvalidDataException("This name is already in use");
            }
            if (!(obj is User))
            {
                var user = DataReader.Get<User>(obj.UserId);
                if (user == null)
                {
                    Logger.Debug("Validating Edit base object error: User id does not exist");
                    throw new InvalidDataException("User id does not exist");
                }
            }
            Logger.Debug("Validating Edit base object Ok");
        }

        protected void ValidateDelete<T>(object data) where T : DataBase
        {
            Logger.Debug("Validating Delete base object");
            var obj = data as DataBase;
            var res = DataReader.Get<T>(obj.Id);
            if (res == null)
            {
                Logger.Debug("Validating Delete base object error: Item does not exist or has already been deleted");
                throw new InvalidDataException("Item does not exist or has already been deleted");
            }
            if (!(obj is User))
            {
                var user = DataReader.Get<User>(obj.UserId);
                if (user == null)
                {
                    Logger.Debug("Validating Delete base object error: User id does not exist");
                    throw new InvalidDataException("User id does not exist");
                }
            }
            Logger.Debug("Validating Delete base object Ok");
        }
    }
}