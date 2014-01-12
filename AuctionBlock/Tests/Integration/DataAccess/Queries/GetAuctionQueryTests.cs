using System;
using System.Collections.Generic;
using AuctionBlock.DataAccess.Commands;
using AuctionBlock.DataAccess.Queries;
using AuctionBlock.Domain.Model;
using FluentAssertions;
using NUnit.Framework;
using StructureMap;

namespace AuctionBlock.Tests.Integration.DataAccess.Queries
{
    public class GetAuctionQueryTests : InMemoryDatabaseTest
    {
        [ExpectedException(typeof(KeyNotFoundException))]
        [Test]
        public void GetAuction_missing_id_should_throw_exception()
        {
            // Arrange
            const string expectedTitle = "Expected title";
            const string expectedDescription = "Expected description";
            var expectedItems = new List<Item> { new Item(expectedDescription) };

            var startAuctionCommand = ObjectFactory.GetInstance<IStartAuctionCommand>();
            startAuctionCommand.Auction = new Auction(expectedTitle, expectedItems);
            TransactionScope(startAuctionCommand.Execute);

            var target = ObjectFactory.GetInstance<IGetAuctionQuery>();
            target.Id = Guid.NewGuid();

            // Act
            target.Execute();
        }

        [Test]
        public void GetAuction_should_return_auction()
        {
            // Arrange
            const string expectedTitle = "Expected title";
            const string expectedDescription = "Expected description";
            var expectedItems = new List<Item> { new Item(expectedDescription) };

            var startAuctionCommand = ObjectFactory.GetInstance<IStartAuctionCommand>();
            var expectedAuction = new Auction(expectedTitle, expectedItems);
            startAuctionCommand.Auction = expectedAuction;
            TransactionScope(startAuctionCommand.Execute);

            var target = ObjectFactory.GetInstance<IGetAuctionQuery>();
            target.Id = expectedAuction.Id;

            // Act
            var actual = target.Execute();

            // Assert
            actual.Id.ShouldBeEquivalentTo(expectedAuction.Id);
            actual.Title.ShouldBeEquivalentTo(expectedAuction.Title);
        }
    }
}