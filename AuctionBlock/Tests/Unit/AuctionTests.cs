using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AuctionBlock.Domain.Model;
using FluentAssertions;
using NUnit.Framework;

namespace AuctionBlock.Tests.Unit
{
    [TestFixture]
    public class AuctionTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Creating_Auction_with_null_items_throws_exception()
        {
            // Assert
            new Auction("Test title", null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Creating_Auction_with_empty_title_throws_exception()
        {
            // Assert
            new Auction(
                string.Empty, 
                new List<Item> { new Item("Test Description") });
        }

        [Test]
        public void Creating_Auction_without_openingPrice_defaults_OpeningPrice_to_0()
        {
            // Act
            var auction = new Auction(
                "Test Title",
                new List<Item> { new Item("Test Description") });

            // Assert
            auction.OpeningPrice.Should().Be(0);
        }

        [ExpectedException(typeof(ConfigurationErrorsException))]
        [Test]
        public void Creating_Auction_with_invalid_openingPrice_thorws_exception()
        {
            // Assert
            const int invalidPrice = -1;
            new Auction(
                "Test Title",
                new List<Item> { new Item("Test Description") },
                invalidPrice);
        }

        [Test]
        public void Can_place_a_bid()
        {
            // Arrange
            var target = new Auction(
                    "Test Auction",
                    new List<Item> { new Item("Test Item Description") });
            var expectedBidder = new Bidder("Test Bidder");
            const decimal expectedAmount = 500.00m;
            const int expectedNumberOfBids = 1;

            // Act
            target.PlaceBid(expectedBidder.Id, expectedAmount);

            //Assert
            target.Bids.Should().NotBeEmpty();
            target.Bids.Count().Should().Be(expectedNumberOfBids);
            Assert.IsTrue(
                target.Bids.Any(
                    x => x.Bidder.Id == expectedBidder.Id
                    && x.Amount == expectedAmount));
        }

        [ExpectedException(typeof(ArgumentException))]
        [Test]
        public void Adding_a_bid_of_a_lower_value_throws_an_exception()
        {
            // Arrange
            var target = new Auction(
                    "Test Auction",
                    new List<Item> { new Item("Test Item Description") });
            var firstBidder = new Bidder("First Bidder");
            const decimal firstAmount = 500.00m;
            var secondBidder = new Bidder("Second Bidder");
            const decimal secondLowerAmount = 400.00m;
            target.PlaceBid(firstBidder.Id, firstAmount);

            // Act
            target.PlaceBid(secondBidder.Id, secondLowerAmount);
        }

        [Test]
        public void Adding_2_bids_should_have_second_bid_as_current_bid()
        {
            // Arrange
            var target = new Auction(
                    "Test Auction",
                    new List<Item> { new Item("Test Item Description") });
            var firstBidder = new Bidder("First Bidder");
            const decimal firstAmount = 500.00m;
            var expectedBidder = new Bidder("Second Bidder");
            const decimal expectedAmount = 600.00m;
            target.PlaceBid(firstBidder.Id, firstAmount);

            // Act
            target.PlaceBid(expectedBidder.Id, expectedAmount);

            // Assert
            target.CurrentBid.Bidder.Id.ShouldBeEquivalentTo(expectedBidder.Id);
            target.CurrentBid.Amount.ShouldBeEquivalentTo(expectedAmount);
        }

        [Test]
        public void Ending_an_auction_will_set_status_to_completed()
        {
            // Arrange
            var target = new Auction(
                    "Test Auction",
                    new List<Item> { new Item("Test Item Description") });
            var expectedBidder = new Bidder("Test Bidder");
            const decimal expectedAmount = 500.00m;
            target.PlaceBid(expectedBidder.Id, expectedAmount);

            // Act
            target.EndAuction();

            // Assert
            target.Status.ShouldBeEquivalentTo(AuctionStatus.Completed);
        }
    }
}