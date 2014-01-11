using System;
using System.Collections.Generic;
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
        public void Creating_Auction_without_configuration_should_throw_exception()
        {
            // Assert
            // ReSharper disable ObjectCreationAsStatement
            new Auction(null);
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Can_place_a_bid()
        {
            // Arrange
            var target = new Auction(
                new Auction.Configuration(
                    "Test Auction",
                    new List<Item> { new Item("Test Item Description") }));
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
            var target = new Auction(new Auction.Configuration(
                    "Test Auction",
                    new List<Item> { new Item("Test Item Description") }));
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
            var target = new Auction(new Auction.Configuration(
                    "Test Auction",
                    new List<Item> { new Item("Test Item Description") }));
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
            var target = new Auction(new Auction.Configuration(
                    "Test Auction",
                    new List<Item> { new Item("Test Item Description") }));
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