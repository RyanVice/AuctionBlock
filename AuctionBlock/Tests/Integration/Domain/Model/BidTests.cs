using System;
using AuctionBlock.Domain.Model;
using FluentNHibernate.Testing;
using NUnit.Framework;

namespace AuctionBlock.Tests.Integration.Domain.Model
{
    public class BidTests : InMemoryDatabaseTest
    {
        [Test]
        public void MapTest()
        {
            new PersistenceSpecification<Bid>(Session)
                .CheckProperty(x => x.Amount, 123.45m)
                .CheckProperty(x => x.PlacedOn, new DateTimeOffset(DateTime.Now))
                .CheckReference(x => x.Bidder, new Bidder("test biddeer"))
                .CheckReference(
                    x => x.Auction, 
                    new Auction(
                        new Auction.Configuration(
                            "test auction", new[] {new Item("test item"), })))
                .VerifyTheMappings();
        }
    }
}