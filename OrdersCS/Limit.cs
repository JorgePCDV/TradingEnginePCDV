using System.Collections.Generic;

namespace TradingEngineServer.Orders
{
    public class Limit
    {
        public long Price { get; set; }
        public OrderbookEntry Head { get; set; }
        public OrderbookEntry Tail { get; set; }


        public uint GetLevelOrderCount()
        {
            uint orderCount = 0;
            OrderbookEntry headPointer = Head;
            while (headPointer != null)
            {
                if(headPointer.CurrentOrder.CurrentQuantity != 0)
                {
                    orderCount++;
                }
                headPointer = headPointer.Next;
            }

            return orderCount;
        }
        public uint GetLevelOrderQuantity()
        {
            uint orderQuantity = 0;
            OrderbookEntry headPointer = Head;
            while (headPointer != null)
            {
                
                orderQuantity += headPointer.CurrentOrder.CurrentQuantity;
                headPointer = headPointer.Next;
            }

            return orderQuantity;
        }

        public List<OrderRecord> GetLevelOrderRecords()
        {
            List<OrderRecord> orderRecords = new List<OrderRecord>();
            OrderbookEntry headPointer = Head;
            uint theoreticalQueuePosition = 0;
            while(headPointer != null)
            {
                var currentOrder = headPointer.CurrentOrder;
                if(currentOrder.CurrentQuantity != 0)
                {
                    orderRecords.Add(
                        new OrderRecord(currentOrder.OrderId, currentOrder.CurrentQuantity, Price, 
                        currentOrder.IsBuySide, currentOrder.Username, currentOrder.SecurityId, theoreticalQueuePosition)
                    );
                    theoreticalQueuePosition++;
                    headPointer = Head.Next;
                }
            }

            return orderRecords;
        }

        public bool IsEmpty
        {
            get 
            {
                return Head == null && Tail == null;
            }
        }

        public Side Side
        {
            get
            {
                if (IsEmpty)
                {
                    return Side.Unknown;
                } else
                {
                    return Head.CurrentOrder.IsBuySide ? Side.Bid : Side.Ask;
                }
            }
        }
    }
}
