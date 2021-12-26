using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public class Orderbook : IRetrievalOrderbook
    {
        public int Count => throw new NotImplementedException();

        public void AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void ChangeOrder(ModifyOrder modifyOrder)
        {
            throw new NotImplementedException();
        }

        public bool ContainsOrder(long OrderId)
        {
            throw new NotImplementedException();
        }

        public List<OrderbookEntry> GetAskOrders()
        {
            throw new NotImplementedException();
        }

        public List<OrderbookEntry> GetBidOrders()
        {
            throw new NotImplementedException();
        }

        public OrderbookSpread GetSpread()
        {
            throw new NotImplementedException();
        }

        public void RemoveOrder(CancelOrder cancelOrder)
        {
            throw new NotImplementedException();
        }
    }
}
