using Models.Enums;

using System;

namespace Models.Business.BusMessages
{
    public class BaseMessage<T>
    {
        public BusMessageType MessageType { get; set; }
        public DateTimeOffset CreationPoint { get; set; }
        public T Content { get; set; }
        public string CreatorName { get; set; }
    }
}
