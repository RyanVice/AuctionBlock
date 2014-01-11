using System;
using System.Collections.Generic;
using System.Linq;
using AuctionBlock.Domain.Model;
using AuctionBlock.Domain.Services;
using AuctionBlock.Registrys;
using FluentAssertions;
using NUnit.Framework;
using StructureMap;

namespace AuctionBlock.Tests.Integration.Domain.Services
{
    [TestFixture]
    public class AuctionBlockServiceTests : InMemoryDatabaseTest
    {
        protected override void SetUp()
        {
            ObjectFactory.Configure(x =>
                {
                    x.AddRegistry<DomainRegistry>();
                    x.AddRegistry<InfrastructureRegistry>();
                    x.AddRegistry<DataAccessRegistry>();
                });
        }

        [Test]
        public void StartAuction_should_create_new_auction_and_add_it_to_cache()
        {
            // Arrange
            const string expectedTitle = "Expected title";
            const string expectedDescription = "Expected description";
            var expectedItems = new List<Item> { new Item(expectedDescription) };
            var target = ObjectFactory.GetInstance<IAuctionBlockService>();

            // Act
            Auction actualAuction = null;
            TransactionScope(() => 
                actualAuction = 
                    target.StartAuction(
                        new Auction.Configuration(expectedTitle, expectedItems)));
            

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
            var expectedItems = new List<Item> { new Item(expectedDescription) };
            var target = ObjectFactory.GetInstance<IAuctionBlockService>();

            // Act
            TransactionScope(() =>
                target.StartAuction(
                    new Auction.Configuration(expectedTitle, expectedItems)));

            // Assert
            target.GetAuction(Guid.NewGuid());
        }

        [Test]
        public void GetAuction_should_return_auction()
        {
            // Arrange
            const string expectedTitle = "Expected title";
            const string expectedDescription = "Expected description";
            var expectedItems = new List<Item> { new Item(expectedDescription) };
            var target = ObjectFactory.GetInstance<IAuctionBlockService>();

            Auction expectedAuction = null;
            TransactionScope(() =>
                expectedAuction = 
                    target.StartAuction(
                        new Auction.Configuration(expectedTitle, expectedItems)));

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
            var expectedItems = new List<Item> { new Item(expectedDescription1) };
            const string expectedTitle2 = "Expected title2";
            const string expectedDescription2 = "Expected description2";
            var expectedItems2 = new List<Item> { new Item(expectedDescription1) };
            var target = ObjectFactory.GetInstance<IAuctionBlockService>();

            IList<Auction> expectedAuctions = new List<Auction>();
            TransactionScope(() =>
                {
                    expectedAuctions.Add(
                        target.StartAuction(
                            new Auction.Configuration(expectedTitle1, expectedItems)));
                    expectedAuctions.Add(
                        target.StartAuction(
                            new Auction.Configuration(expectedTitle2, expectedItems2)));
                    var inactiveAuction = target.StartAuction(
                        new Auction.Configuration("Inactive title", new List<Item> { new Item("Description")}));
                    target.EndAuction(inactiveAuction.Id);
                });

            // Act
            var actual = target.GetActiveAuctions();

            // Assert
            actual.Count().Should().Be(expectedAuctions.Count);
        }
    }
}