using System;

namespace TradingEngineServer.Orderbook
{
    public interface IReadOnlyOrderbook
    {
        bool ContainsOrder(long OrderId);
        OrderbookSpread GetSpread();
    }
}
