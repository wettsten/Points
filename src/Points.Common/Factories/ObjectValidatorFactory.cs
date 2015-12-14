using System;
using System.Collections.Generic;
using System.Linq;
using Points.Common.Validators;

namespace Points.Common.Factories
{
    public class ObjectValidatorFactory : IObjectValidatorFactory
    {
        private readonly IDictionary<Type, IObjectValidator> _validators;

        public ObjectValidatorFactory(IEnumerable<IObjectValidator> validators)
        {
            _validators = validators.ToDictionary(validator => validator.SupportsType, validator => validator);
        }

        public IObjectValidator Get(Type objType)
        {
            return _validators.ContainsKey(objType) ? _validators[objType] : null;
        }
    }
}