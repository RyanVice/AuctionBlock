using System;

namespace AuctionBlock.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static void ThrowIfNull(this object @this, string message)
        {
            if (@this == null)
                throw new ArgumentNullException(message);
        }

        public static void ThrowIfNull<TException>(this object @this, TException exception)
            where TException : Exception
        {
            if (@this == null)
                throw exception;
        }
    }
}