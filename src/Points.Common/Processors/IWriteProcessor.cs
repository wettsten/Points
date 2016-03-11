using Points.Model;

namespace Points.Common.Processors
{
    public interface IWriteProcessor
    {
        void AddData<TView>(TView data, string userId) where TView : ModelBase;
        void EditData<TView>(TView data, string userId) where TView : ModelBase;
        void DeleteData<TView>(TView data, string userId) where TView : ModelBase;
    }
}
