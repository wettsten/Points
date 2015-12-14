using System;

namespace Points.Common.Validators
{
    public interface IObjectValidator
    {
        Type SupportsType { get; }
        void ValidateAdd(object data);
        void ValidateEdit(object data);
        void ValidateDelete(object data);
    }
}