using System;
using System.Linq;
using AutoMapper;
using Points.Common.Extensions;
using Points.DataAccess.Readers;

namespace Points.Common.AutoMapper
{
    public class MappingProfile : Profile
    {
        private readonly IDataReader _dataReader;

        // For unit testing
        public MappingProfile() { }

        public MappingProfile(IDataReader dataReader)
        {
            _dataReader = dataReader;
        }

        protected override void Configure()
        {
            // View to Raven
            CreateMap<Model.ModelBase, Data.DataBase>()
                .ForMember(t => t.UserId, o => o.Ignore());
            CreateMap<Model.Duration, Data.Duration>()
                .ForMember(t => t.Type, o => o.MapFrom(s => (Data.DurationType)Enum.Parse(typeof(Data.DurationType), s.Type.Id)))
                .ForMember(t => t.Unit, o => o.MapFrom(s => (Data.DurationUnit)Enum.Parse(typeof(Data.DurationUnit), s.Unit.Id)));
            CreateMap<Model.Frequency, Data.Frequency>()
                .ForMember(t => t.Type, o => o.MapFrom(s => (Data.FrequencyType)Enum.Parse(typeof(Data.FrequencyType), s.Type.Id)))
                .ForMember(t => t.Unit, o => o.MapFrom(s => (Data.FrequencyUnit)Enum.Parse(typeof(Data.FrequencyUnit), s.Unit.Id)));
            CreateMap<Model.Category, Data.Category>();
            CreateMap<Model.Task, Data.Task>()
                .ForMember(t => t.CategoryId, o => o.MapFrom(s => s.Category.Id));
            CreateMap<Model.PlanningTask, Data.PlanningTask>()
                .ForMember(t => t.TaskId, o => o.MapFrom(s => s.Task.Id));
            CreateMap<Model.ActiveTask, Data.ActiveTask>()
                .ForMember(t => t.TaskId, o => o.MapFrom(s => s.Task.Id));
            CreateMap<Model.ArchivedTask, Data.ArchivedTask>()
                .ForMember(t => t.TaskId, o => o.MapFrom(s => s.Task.Id));
            CreateMap<Model.User, Data.User>()
                .ForMember(t => t.WeekStartHour, o => o.MapFrom(s => s.WeekStartHour.Id))
                .ForMember(t => t.NotifyWeekStarting, o => o.MapFrom(s => s.NotifyWeekStarting.Id))
                .ForMember(t => t.NotifyWeekEnding, o => o.MapFrom(s => s.NotifyWeekEnding.Id));
            CreateMap<Model.Job, Data.Job>();

            // Raven to View
            CreateMap<Data.DataBase, Model.ModelBase>();
            CreateMap<Data.Duration, Model.Duration>()
                .ForMember(t => t.Type, o => o.MapFrom(s => new Model.ModelBase {Id = s.Type.ToString(), Name = s.Type.Spacify()}))
                .ForMember(t => t.Unit, o => o.MapFrom(s => new Model.ModelBase { Id = s.Unit.ToString(), Name = s.Unit.Spacify()}));
            CreateMap<Data.Frequency, Model.Frequency>()
                .ForMember(t => t.Type, o => o.MapFrom(s => new Model.ModelBase { Id = s.Type.ToString(), Name = s.Type.Spacify()}))
                .ForMember(t => t.Unit, o => o.MapFrom(s => new Model.ModelBase { Id = s.Unit.ToString(), Name = s.Unit.Spacify()}));
            CreateMap<Data.Category, Model.Category>();
            CreateMap<Data.Task, Model.Task>()
                .ForMember(t => t.Category, o => o.MapFrom(s => _dataReader.Get<Data.Category>(s.CategoryId)));
            CreateMap<Data.PlanningTask, Model.PlanningTask>()
                .ForMember(t => t.Task, o => o.MapFrom(s => _dataReader.Get<Data.Task>(s.TaskId)));
            CreateMap<Data.ActiveTask, Model.ActiveTask>()
                .ForMember(t => t.Task, o => o.MapFrom(s => _dataReader.Get<Data.Task>(s.TaskId)));
            CreateMap<Data.ArchivedTask, Model.ArchivedTask>()
                .ForMember(t => t.Task, o => o.MapFrom(s => _dataReader.Get<Data.Task>(s.TaskId)));
            CreateMap<Data.User, Model.User>()
                .ForMember(t => t.PlanningEndTime, o => o.MapFrom(s => GetPlanningEndTime(s)))
                .ForMember(t => t.ActiveStartTime, o => o.MapFrom(s => GetActiveStartTime(s)))
                .ForMember(t => t.WeekStartHour, o => o.MapFrom(s => Model.SimpleInt.FromId(s.WeekStartHour)))
                .ForMember(t => t.NotifyWeekStarting, o => o.MapFrom(s => Model.SimpleInt.FromId(s.NotifyWeekStarting)))
                .ForMember(t => t.NotifyWeekEnding, o => o.MapFrom(s => Model.SimpleInt.FromId(s.NotifyWeekEnding)));
            CreateMap<Data.Job, Model.Job>();
        }

        private DateTime? GetPlanningEndTime(Data.User user)
        {
            var job = _dataReader.GetAll<Data.Job>().FirstOrDefault(i => i.UserId.Equals(user.Id) && i.Processor.Equals("StartWeekJob"));
            if (job == null)
            {
                return new DateTime?();
            }
            return job.Trigger;
        }

        private DateTime? GetActiveStartTime(Data.User user)
        {
            var job = _dataReader.GetAll<Data.Job>().FirstOrDefault(i => i.UserId.Equals(user.Id) && i.Processor.Equals("EndWeekJob"));
            if (job == null)
            {
                return new DateTime?();
            }
            return job.Trigger;
        }
    }
}