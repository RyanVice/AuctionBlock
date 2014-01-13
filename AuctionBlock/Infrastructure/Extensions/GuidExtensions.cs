using System;

namespace AuctionBlock.Infrastructure.Extensions
{
    public static class GuidExtensions
    {
        public static void ThrowIfNullOrEmpty(this Guid @this, string message)
        {
            if (@this == null)
                throw new ArgumentNullException(message);

            if (@this == Guid.Empty)
                throw new ArgumentException(message);
        }
    }
}