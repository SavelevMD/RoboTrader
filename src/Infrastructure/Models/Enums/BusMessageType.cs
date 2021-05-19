using System;

namespace Models.Enums
{
    public enum BusMessageType : UInt16
    {
        Tasks = 1,
        Candles = 2,
        Indicators = 3,
        SubscribeChannel = 4,
        UnsubscribeChannel = 5,
        BuildIndicators = 6,
        Errors = 500
    }
}
