using System.Collections.Generic;
using System.Linq;
using AuctionBlock.DataAccess.Commands;
using AuctionBlock.DataAccess.Queries;
using AuctionBlock.Domain.Model;
using FluentAssertions;
using NUnit.Framework;
using StructureMap;

namespace AuctionBlock.Tests.Integration.DataAccess.Commands
{
    [TestFixture]
    public class StartAuctionCommandTests : InMemoryDatabaseTest
    {
        [Test]
        public void StartAuction_should_create_new_auction_and_add_it_to_cache()
        {
            // Arrange
            const string expectedTitle = "Expected title";
            const string expectedDescription = "Expected description";
            var expectedItems = new List<Item> { new Item(expectedDescription) };
            var target = ObjectFactory.GetInstance<IStartAuctionCommand>();
            var actualAuction = new Auction(expectedTitle, expectedItems);
            target.Auction = actualAuction;

            // Act
            TransactionScope(target.Execute);


            // Assert
            var actualAuctionsInCache 
                = ObjectFactory.GetInstance<IGetActiveAuctionsQuery>()
                               .Execute().ToList();
            actualAuctionsInCache.Count().ShouldBeEquivalentTo(1);
            actualAuctionsInCache.First().Id.ShouldBeEquivalentTo(actualAuction.Id);
        }
    }
}