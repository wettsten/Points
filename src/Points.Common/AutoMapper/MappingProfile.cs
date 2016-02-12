using AutoMapper;
using Points.DataAccess.Readers;

namespace Points.Common.AutoMapper
{
    public class MappingProfile : Profile
    {
        private readonly DataReader _dataReader;

        public MappingProfile(DataReader dataReader)
        {
            _dataReader = dataReader;
        }

        protected override void Configure()
        {
            // View to Raven
            CreateMap<Data.View.ViewObject, Data.Raven.RavenObject>();
            CreateMap<Data.View.Category, Data.Raven.Category>();
            CreateMap<Data.View.Task, Data.Raven.Task>()
                .ForMember(t => t.CategoryId, o => o.MapFrom(s => s.Category.Id));
            CreateMap<Data.View.PlanningTask, Data.Raven.PlanningTask>()
                .ForMember(t => t.TaskId, o => o.MapFrom(s => s.Task.Id));
            CreateMap<Data.View.ActiveTask, Data.Raven.ActiveTask>()
                .ForMember(t => t.TaskId, o => o.MapFrom(s => s.Task.Id));
            CreateMap<Data.View.ArchivedTask, Data.Raven.ArchivedTask>()
                .ForMember(t => t.TaskId, o => o.MapFrom(s => s.Task.Id));
            CreateMap<Data.View.User, Data.Raven.User>();
            CreateMap<Data.View.Job, Data.Raven.Job>();

            // Raven to View
            CreateMap<Data.Raven.RavenObject, Data.View.ViewObject>();
            CreateMap<Data.Raven.Category, Data.View.Category>();
            CreateMap<Data.Raven.Task, Data.View.Task>()
                .ForMember(t => t.Category, o => o.MapFrom(s => _dataReader.Get<Data.Raven.Category>(s.CategoryId)));
            CreateMap<Data.Raven.PlanningTask, Data.View.PlanningTask>()
                .ForMember(t => t.Task, o => o.MapFrom(s => _dataReader.Get<Data.Raven.Task>(s.TaskId)));
            CreateMap<Data.Raven.ActiveTask, Data.View.ActiveTask>()
                .ForMember(t => t.Task, o => o.MapFrom(s => _dataReader.Get<Data.Raven.Task>(s.TaskId)));
            CreateMap<Data.Raven.ArchivedTask, Data.View.ArchivedTask>()
                .ForMember(t => t.Task, o => o.MapFrom(s => _dataReader.Get<Data.Raven.Task>(s.TaskId)));
            CreateMap<Data.Raven.User, Data.View.User>();
            CreateMap<Data.Raven.Job, Data.View.Job>();
        }
    }
}