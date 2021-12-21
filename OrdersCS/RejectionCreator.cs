using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Rejects
{
    public sealed class RejectionCreator
    {
        public static Rejection GenerateOrderCoreRejection(IOrderCore rejectedOrder, RejectionReason rejectionReason)
        {
            return new Rejection(rejectedOrder, rejectionReason);
        }
    }
}
