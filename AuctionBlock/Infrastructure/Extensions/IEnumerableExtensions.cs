using System;
using System.Collections;
using NHibernate.Util;
using Remotion.Linq.Utilities;

namespace AuctionBlock.Infrastructure.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ThrowIfNullOrEmpty<T>(this T @this, string message)
            where T : IEnumerable
        {
            if (@this == null)
                throw new ArgumentNullException(message);
            if (!@this.Any())
                throw new ArgumentEmptyException(message);
        }
    }
}