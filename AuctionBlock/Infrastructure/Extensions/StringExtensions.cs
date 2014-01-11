using System;
using System.Configuration;

namespace AuctionBlock.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static void ThrowIfNullEmptyOrWhiteSpace(this string @this, string message)
        {
            if (string.IsNullOrWhiteSpace(@this))
                throw new ArgumentNullException(message);
        }
    }

    public static class DecimalExtension
    {
        public static void ThrowConfigurationExceptionIf(
            this decimal @this, 
            Func<decimal, bool> isValid,
            string message)
        {
            if (!isValid.Invoke(@this))
                throw new ConfigurationErrorsException(message);
        }
    }
}