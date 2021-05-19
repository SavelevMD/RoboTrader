using System.Reactive.Subjects;

using BitfinexAdapter.Logic.Models;

using Models.Results;

namespace BitfinexAdapter.Logic.Services
{
    internal interface ISocketCommandService
    {
        bool IsMaintenanceMode { get; set; }
        Subject<IStockCommand> CommandSubject { get; }
        public Subject<bool> ReconnectIsNeeded { get; }
        OperationResult SendCommand(string command);
        OperationResult ReconnectCommand();
        void RefreshSubscriptions();
    }
}