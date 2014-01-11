using System;
using System.Collections.Generic;
using System.Configuration;
using AuctionBlock.Domain.Model;
using NUnit.Framework;

namespace AuctionBlock.Tests.Unit
{
    [TestFixture]
    public class AuctionConfigurationTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Creating_AuctionConfiguration_with_null_lot_throws_exception()
        {
            // Assert
            new Auction.Configuration("Test title", null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Creating_AuctionConfiguration_with_empty_title_throws_exception()
        {
            // Assert
            new Auction.Configuration(
                string.Empty, new List<Item> { new Item("Test Description") });
        }

        [ExpectedException(typeof(ConfigurationErrorsException))]
        [Test]
        public void Creating_configuation_with_invalid_interval_throws_exception()
        {
            // Arrange
            const int invalidIncrement = -1;

            // Act
            var configuraiton 
                = new Auction.Configuration(
                    "Test title", 
                    new List<Item> { new Item("Test description") })
                {
                    Increment = invalidIncrement
                };
            configuraiton.Configure(new Auction(configuraiton));
        }
    }
}