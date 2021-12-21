using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class Order : IOrderCore
    {
        public Order(IOrderCore orderCore, long price, uint quantity, bool isBuySide)
        {
            // PROPERTIES //
            Price = price;
            InitialQuantity = quantity;
            CurrentQuantity = quantity;
            IsBuySide = isBuySide;

            // FIELDS //
            _orderCore = orderCore;
        }

        // PROPERTIES //
        public long Price { get; private set; }
        public uint InitialQuantity { get; private set; }
        public uint CurrentQuantity { get; private set; }
        public bool IsBuySide { get; private set; }

        public long OrderId => throw new NotImplementedException();

        public string Username => throw new NotImplementedException();

        public int SecurityId => throw new NotImplementedException();

        // METHODS //
        public void IncreaseQuantity(uint quantityDelta)
        {
            CurrentQuantity += quantityDelta;
        }

        public void DecreaseQuantity(uint quantityDelta)
        {
            if(quantityDelta > CurrentQuantity)
            {
                throw new InvalidOperationException(
                    $"Quantity Delta > Current Quantity for OrderId={OrderId}"
                );
            }
            CurrentQuantity -= quantityDelta;
        }

        // FIELDS //
        private readonly IOrderCore _orderCore;
    }
}
