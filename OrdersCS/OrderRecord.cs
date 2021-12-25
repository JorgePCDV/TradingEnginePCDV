using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    /// <summary>
    /// Read-only representation of an order
    /// </summary>
    public record OrderRecord(long OrderId, uint Quantity, long Price, bool IsBuySide, 
        string Username, int SecurityId, uint TheoreticalQueuePosition);
}

/// <summary>
/// This is to enable record types in C#9 due to Visual Studio 2019 bug.
/// </summary>
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {};
}