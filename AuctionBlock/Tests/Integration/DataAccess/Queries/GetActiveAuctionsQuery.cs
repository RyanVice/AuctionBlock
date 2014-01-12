using System.Collections.Generic;
using System.Linq;
using AuctionBlock.DataAccess.Commands;
using AuctionBlock.DataAccess.Queries;
using AuctionBlock.Domain.Model;
using FluentAssertions;
using NUnit.Framework;
using StructureMap;

namespace AuctionBlock.Tests.Integration.DataAccess.Queries
{
    [TestFixture]
    public class GetActiveAuctionsQuery : InMemoryDatabaseTest
    {
        [Test]
        public void GetAuction_should_return_active_auction()
        {
            // Arrange
            const string expectedTitle1 = "Expected title1";
            const string expectedDescription1 = "Expected description1";
            var expectedItems1 = new List<Item> { new Item(expectedDescription1) };
            var expectedAuction1 = new Auction(expectedTitle1, expectedItems1);
            const string expectedTitle2 = "Expected title2";
            var expectedItems2 = new List<Item> { new Item(expectedDescription1) };
            var expectedAuction2 = new Auction(expectedTitle2, expectedItems2);
            var startAuctionCommand = ObjectFactory.GetInstance<IStartAuctionCommand>();
            var target = ObjectFactory.GetInstance<IGetActiveAuctionsQuery>();

            IList<Auction> expectedAuctions = new List<Auction>();
            TransactionScope(() =>
                {
                    startAuctionCommand.Auction = expectedAuction1;
                    startAuctionCommand.Execute();
                    expectedAuctions.Add(expectedAuction1);

                    startAuctionCommand.Auction = expectedAuction2;
                    startAuctionCommand.Execute();
                    expectedAuctions.Add(expectedAuction2);

                    var inactiveAuction 
                        = new Auction(
                            "Inactive title", 
                            new List<Item> { new Item("Description") });
                    startAuctionCommand.Auction = inactiveAuction;
                    startAuctionCommand.Execute();

                    inactiveAuction.EndAuction();
                });

            // Act
            var actual = target.Execute();

            // Assert
            actual.Count().Should().Be(expectedAuctions.Count);
        }

    }
}