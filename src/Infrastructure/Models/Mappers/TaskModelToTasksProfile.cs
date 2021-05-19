using AutoMapper;
using Models.Business;

namespace Models.Mappers
{
    public class TaskModelToTasksProfile : Profile
    {
        public TaskModelToTasksProfile()
        {
            CreateMap<TaskModel, Robo.Database.Models.Tasks>()
                .ForMember(r => r.Id, u => u.MapFrom(r => r.Id))
                .ForMember(r => r.Body, u => u.MapFrom(r => r.Body))
                .ForMember(r => r.CreateDate, u => u.MapFrom(r => r.CreateDate))
                .ForMember(r => r.IsActive, u => u.MapFrom(r => r.IsActive))
                .ForMember(r => r.TaskName, u => u.MapFrom(r => r.TaskName)).ReverseMap();
        }
    }
}
