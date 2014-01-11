using AuctionBlock.Domain.Model;
using FluentNHibernate.Testing;
using NUnit.Framework;

namespace AuctionBlock.Tests.Integration.Domain.Model
{
    public class BidderTests : InMemoryDatabaseTest
    {
        [Test]
        public void MapTest()
        {
            new PersistenceSpecification<Bidder>(Session)
                .CheckProperty(x => x.Name, "name")
                .VerifyTheMappings();
        }
    }
}