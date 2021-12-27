using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Instrument;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public class Orderbook : IRetrievalOrderbook
    {
        // PRIVATE FIELDS //
        private readonly Security _instrument;
        private readonly Dictionary<long, OrderbookEntry> _orders = new Dictionary<long, OrderbookEntry>();
        private readonly SortedSet<Limit> _askLimits = new SortedSet<Limit>(AskLimitComparer.Comparer);
        private readonly SortedSet<Limit> _bidLimits = new SortedSet<Limit>(BidLimitComparer.Comparer);

        public Orderbook(Security instrument)
        {
            _instrument = instrument;
        }

        public int Count => _orders.Count;

        public void AddOrder(Order order)
        {
            var baseLimit = new Limit(order.Price);
            AddOrder(order, baseLimit, order.IsBuySide ? _bidLimits : _askLimits, _orders);
        }

        private static void AddOrder(Order order, Limit baseLimit, SortedSet<Limit> limitLevels, Dictionary<long, OrderbookEntry> internalBook)
        {
            if (limitLevels.TryGetValue(baseLimit, out Limit limit))
            {
                OrderbookEntry orderbookEntry = new OrderbookEntry(order, baseLimit);
                if(limit.Head == null)
                {
                    limit.Head = orderbookEntry;
                    limit.Tail = orderbookEntry;
                } else
                {
                    OrderbookEntry tailPointer = limit.Tail;
                    tailPointer.Next = orderbookEntry;
                    orderbookEntry.Previous = tailPointer;
                    limit.Tail = orderbookEntry;
                }
                internalBook.Add(order.OrderId, orderbookEntry);
            }
            else
            {
                limitLevels.Add(baseLimit);
                OrderbookEntry orderbookEntry = new OrderbookEntry(order, baseLimit);
                baseLimit.Head = orderbookEntry;
                baseLimit.Tail = orderbookEntry;
                internalBook.Add(order.OrderId, orderbookEntry);
            }
        }

        public void ChangeOrder(ModifyOrder modifyOrder)
        {
            if(_orders.TryGetValue(modifyOrder.OrderId, out OrderbookEntry orderBookEntry))
            {
                RemoveOrder(modifyOrder.ToCancelOrder());
                AddOrder(modifyOrder.ToNewOrder(), orderBookEntry.ParentLimit, modifyOrder.IsBuySide ? _bidLimits : _askLimits, _orders);
            }
        }

        public void RemoveOrder(CancelOrder cancelOrder)
        {
            if (_orders.TryGetValue(cancelOrder.OrderId, out OrderbookEntry orderBookEntry))
            {
                RemoveOrder(cancelOrder.OrderId, orderBookEntry, _orders);
            }
        }

        private static void RemoveOrder(long orderId, OrderbookEntry orderBookEntry, Dictionary<long, OrderbookEntry> internalBook)
        {
            // Deal with the location of OrderbookEntry within the LinkedList.
            if(orderBookEntry.Previous != null && orderBookEntry != null)
            {
                orderBookEntry.Next.Previous = orderBookEntry.Previous;
                orderBookEntry.Previous.Next = orderBookEntry.Next;
            }
            else if(orderBookEntry.Previous != null)
            {
                orderBookEntry.Previous.Next = null;
            }
            else if (orderBookEntry.Next != null)
            {
                orderBookEntry.Next.Previous = null;
            }
            
            // Deal with OrderbookEntry on Limit-level.
            if (orderBookEntry.ParentLimit.Head == orderBookEntry && orderBookEntry.ParentLimit.Tail == orderBookEntry)
            {
                // Only one order on this Limit-level.
                orderBookEntry.ParentLimit.Head = null;
                orderBookEntry.ParentLimit.Tail = null;
            } 
            else if (orderBookEntry.ParentLimit.Head == orderBookEntry)
            {
                // More than one order, but orderBookEntry is first order on level.
                orderBookEntry.ParentLimit.Head = orderBookEntry.Next;
            }
            else if (orderBookEntry.ParentLimit.Tail == orderBookEntry)
            {
                // More than one order, but orderBookEntry is last order on level.
                orderBookEntry.ParentLimit.Tail = orderBookEntry.Previous;
            }

            internalBook.Remove(orderId);
        }

        public bool ContainsOrder(long orderId)
        {
            return _orders.ContainsKey(orderId);
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
    }
}
