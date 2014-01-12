using System;
using System.Collections.Generic;
using System.Linq;
using AuctionBlock.Domain.Model;
using FluentAssertions;
using FluentNHibernate.Testing;
using NUnit.Framework;

namespace AuctionBlock.Tests.Integration.Domain.Model
{
    [TestFixture]
    public class AuctionTests : InMemoryDatabaseTest
    {
        [Test]
        public void Can_add_items()
        {
            object id;
            var items = new List<Item>
                    {
                        new Item("item 1"), new Item("item 2"), new Item("item 3")
                    };

            using (var transaction = Session.BeginTransaction())
            {
                id = Session.Save(new Auction("test", items));

                transaction.Commit();
            }

            Session.Clear();

            using (var transaction = Session.BeginTransaction())
            {
                var actual = Session.Get<Auction>(id);

                actual.Items.Count.ShouldBeEquivalentTo(items.Count);
                items.ForEach(x =>
                    actual.Items
                        .Any(
                            y => y.Description == x.Description
                            && x.Id == y.Id)
                        .Should().BeTrue());
                transaction.Commit();
            }
        }

        [Test]
        public void Can_add_bids()
        {
            // Arrange
            object id;
            var items = new List<Item>
                    {
                        new Item("item 1"), new Item("item 2"), new Item("item 3")
                    };
            var auction = new Auction("test", items);
            const decimal expectedBidAmount1 = 123.45m;
            var expectedBidder1 = new Bidder("bidder1");
            var expectedBidder2 = new Bidder("bidder2");

            const decimal expectedBidAmount2 = 321.54m;

            // Act
            using (var transaction = Session.BeginTransaction())
            {
                Session.Save(expectedBidder1);
                Session.Save(expectedBidder2);

                auction.PlaceBid(expectedBidder1.Id, expectedBidAmount1);
                auction.PlaceBid(expectedBidder2.Id, expectedBidAmount2);

                id = Session.Save(auction);

                transaction.Commit();
            }

            Session.Clear();

            using (var transaction = Session.BeginTransaction())
            {
                var actual = Session.Get<Auction>(id);

                // Assert
                actual.Bids.Count.Should().Be(2);
                actual.Bids.ToList().ForEach(x =>
                    actual.Bids
                        .Any(
                            y => y.Amount == x.Amount
                            && x.Id == y.Id)
                        .Should().BeTrue());
                actual.CurrentBid.Amount.Should()
                    .Be(expectedBidAmount2, 
                        "The current bid must be the last bid added");
                actual.CurrentBid.Amount.Should()
                    .Be(actual.Bids.Max(x => x.Amount));
                transaction.Commit();
            }
        }

        [Test]
        public void MapTest()
        {
            new PersistenceSpecification<Auction>(Session)
                .CheckProperty(x => x.Title, "title")
                .CheckProperty(x => x.OpeningPrice, 123.45m)
                .CheckProperty(x => x.StartedAt, new DateTimeOffset(DateTime.Now))
                .CheckProperty(x => x.Status, AuctionStatus.Completed)
                .VerifyTheMappings();
        }
    }
}