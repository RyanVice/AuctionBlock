using AuctionBlock.Domain.Model;
using FluentNHibernate.Testing;
using NUnit.Framework;

namespace AuctionBlock.Tests.Integration.Domain.Model
{
    public class ItemTests : InMemoryDatabaseTest
    {
        [Test]
        public void MapTest()
        {
            new PersistenceSpecification<Item>(Session)
                .CheckProperty(x => x.Description, "description")
                .CheckReference(
                    x => x.Auction,
                    new Auction("test auction", new[] { new Item("test item"), }))
                .VerifyTheMappings();
        }
    }
}