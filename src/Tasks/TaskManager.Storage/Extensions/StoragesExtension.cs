
using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using Models.Mappers;

using TaskManager.Storage.Repositories;
using TaskManager.Storage.Repositories.MemoryStorages;

namespace TaskManager.Storage.Extensions
{
    public static class StoragesExtension
    {
        public static IServiceCollection AddTaskStorages(this IServiceCollection services)
        {
            services.AddSingleton<ITaskNamesCollectionStorage, TaskNamesCollectionStorage>();
            services.AddSingleton<ITaskStorage, TaskStorage>();

            services.AddSingleton<ITaskRepository, TaskRepository>();

            services.AddAutoMapper(config =>
            {
                config.AddProfile<TaskHistoryModelToTaskHistoryProfile>();
                config.AddProfile<TaskModelToTasksProfile>();
            }, typeof(TaskModelToTasksProfile).Assembly);

            return services;
        }
    }
}
