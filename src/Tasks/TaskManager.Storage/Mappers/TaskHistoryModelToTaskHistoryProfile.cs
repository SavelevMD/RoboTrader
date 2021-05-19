using AutoMapper;

using Models.Business;

using Robo.Database.Models;

namespace TaskManager.Storage.Mappers
{
    public class TaskHistoryModelToTaskHistoryProfile : Profile
    {
        public TaskHistoryModelToTaskHistoryProfile()
        {
            CreateMap<TasksHistories, TasksHistoryModel>()
                .ForMember(r => r.Id, u => u.MapFrom(r => r.Id))
                .ForMember(r => r.State, u => u.MapFrom(r => r.State))
                .ForMember(r => r.TimeOfChangeState, u => u.MapFrom(r => r.TimeOfChangeState))
                .ForMember(r => r.TaskId, u => u.MapFrom(r => r.TaskId))
                .ForMember(r => r.StateJson, u => u.MapFrom(r => r.StateJson))
                .ForMember(r => r.Task, u => u.MapFrom(r => r.Task))
                .ForMember(r => r.TaskName, u => u.MapFrom(r => r.TaskName));
        }
    }
}
