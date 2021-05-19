using System.Threading.Tasks;

using Models.Business.BusMessages;
using Models.Tasks;

namespace IndicatorsBuilderService.Logic.Services.Processing
{
    internal interface ITaskProcessing
    {
        void AddTasksProcessingAsync(string channelName, BaseMessage<TaskWrapper> task);
        Task KillTasksProcessingAsync(string channelName, BaseMessage<string> taskName);
    }
}