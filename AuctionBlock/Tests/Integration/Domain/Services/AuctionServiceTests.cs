using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AuctionBlock.Domain.Model;
using AuctionBlock.Domain.Services;
using AuctionBlock.Registrys;
using FluentAssertions;
using NUnit.Framework;
using StructureMap;

namespace AuctionBlock.Tests.Integration.Domain.Services
{
    public class AuctionServiceTests : InMemoryDatabaseTest
    {
        protected override void SetUp()
        {
            ObjectFactory.Configure(x =>
            {
                x.AddRegistry<InfrastructureRegistry>();
                x.AddRegistry<DomainRegistry>();
            });
        }

        [Test]
        public void StartAuction_should_create_new_auction_and_add_it_to_cache()
        {
            // Arrange
            const string expectedTitle = "Expected title";
            const string expectedDescription = "Expected description";
            var expectedItems = new List<Item>
                {
                    new Item(expectedDescription)
                };
            var target = ObjectFactory.GetInstance<IAuctionService>();

            // Act
            Auction actualAuction = null;
            TransactionScope(() =>
                actualAuction =
                    target.StartAuction(expectedTitle, expectedItems));


            // Assert
            var actualAuctionsInCache = target.GetActiveAuctions().ToList();
            actualAuctionsInCache.Count().ShouldBeEquivalentTo(1);
            actualAuctionsInCache.First().Id.ShouldBeEquivalentTo(actualAuction.Id);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [Test]
        public void GetAuction_missing_id_should_throw_exception()
        {
            // Arrange
            const string expectedTitle = "Expected title";
            const string expectedDescription = "Expected description";
            var expectedItems = new List<Item>
                {
                    new Item(expectedDescription)
                };
            var target = ObjectFactory.GetInstance<IAuctionService>();

            // Act
            TransactionScope(() =>
                target.StartAuction(expectedTitle, expectedItems));

            // Assert
            target.GetAuction(Guid.NewGuid());
        }

        [Test]
        public void GetAuction_should_return_auction()
        {
            // Arrange
            const string expectedTitle = "Expected title";
            const string expectedDescription = "Expected description";
            var expectedItems = new List<Item>
                {
                    new Item(expectedDescription)
                };
            var target = ObjectFactory.GetInstance<IAuctionService>();

            Auction expectedAuction = null;
            TransactionScope(() =>
                expectedAuction =
                    target.StartAuction(expectedTitle, expectedItems));

            // Act
            var actual = target.GetAuction(expectedAuction.Id);

            // Assert
            actual.Id.ShouldBeEquivalentTo(expectedAuction.Id);
            actual.Title.ShouldBeEquivalentTo(expectedAuction.Title);
        }

        [Test]
        public void GetAuction_should_return_active_auction()
        {
            // Arrange
            const string expectedTitle1 = "Expected title1";
            const string expectedDescription1 = "Expected description1";
            var expectedItems = new List<Item>
                {
                    new Item(expectedDescription1)
                };
            const string expectedTitle2 = "Expected title2";
            var expectedItems2 = new List<Item>
                {
                    new Item(expectedDescription1)
                };
            var target = ObjectFactory.GetInstance<IAuctionService>();

            IList<Auction> expectedAuctions = new List<Auction>();
            TransactionScope(() =>
            {
                expectedAuctions.Add(
                    target.StartAuction(expectedTitle1, expectedItems));
                expectedAuctions.Add(
                    target.StartAuction(expectedTitle2, expectedItems2));
                var inactiveAuction
                    = target.StartAuction(
                        "Inactive title",
                        new List<Item>
                                {
                                    new Item("Description")
                                });
                target.EndAuction(inactiveAuction.Id);
            });

            // Act
            var actual = target.GetActiveAuctions();

            // Assert
            actual.Count().Should().Be(expectedAuctions.Count);
        }

        [Test]
        public void Can_place_a_bid()
        {
            // Arrange
            var target = ObjectFactory.GetInstance<IAuctionService>();

            var auction = target.StartAuction(
                    "Test Auction",
                    new List<Item>
                        {
                            new Item("Test Item Description")
                        });
            var expectedBidder = new Bidder("Test Bidder");
            const decimal expectedAmount = 500.00m;
            const int expectedNumberOfBids = 1;

            // Act
            target.PlaceBid(auction.Id, expectedBidder.Id, expectedAmount);

            //Assert
            auction.Bids.Should().NotBeEmpty();
            auction.Bids.Count().Should().Be(expectedNumberOfBids);
            Assert.IsTrue(
                auction.Bids.Any(
                    x => x.Bidder.Id == expectedBidder.Id
                    && x.Amount == expectedAmount));
        }

        [Test]
        public void Ending_an_auction_will_set_status_to_completed()
        {
            // Arrange
            var target = ObjectFactory.GetInstance<IAuctionService>();

            var auction = target.StartAuction(
                    "Test Auction",
                    new List<Item> { new Item("Test Item Description") });
            var expectedBidder = new Bidder("Test Bidder");
            const decimal expectedAmount = 500.00m;
            target.PlaceBid(auction.Id, expectedBidder.Id, expectedAmount);

            // Act
            target.EndAuction(auction.Id);

            // Assert
            auction.Status.ShouldBeEquivalentTo(AuctionStatus.Completed);
        }
    }
}