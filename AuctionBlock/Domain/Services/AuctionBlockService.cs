﻿using System;
using System.Collections.Generic;
using AuctionBlock.DataAccess.Commands;
using AuctionBlock.DataAccess.Queries;
using AuctionBlock.Domain.Model;
using AuctionBlock.Infrastructure.Extensions;
using AuctionBlock.Infrastructure.Factories;

namespace AuctionBlock.Domain.Services
{
    public class AuctionBlockService : IAuctionBlockService
    {
        private readonly IFactory<IGetActiveAuctionsQuery> _getActiveAuctionsQueryFactory;
        private readonly IFactory<IGetAuctionQuery> _getAuctionQueryFactory;
        private readonly IFactory<IStartAuctionCommand> _startAuctionCommandFactory;


        public AuctionBlockService(
            IFactory<IGetActiveAuctionsQuery> getActiveAuctionsQueryFactory,
            IFactory<IStartAuctionCommand> startAuctionComandFactory,
            IFactory<IGetAuctionQuery> getAuctionQueryFactory)
        {
            _getActiveAuctionsQueryFactory = getActiveAuctionsQueryFactory;
            _getAuctionQueryFactory = getAuctionQueryFactory;
            _startAuctionCommandFactory = startAuctionComandFactory;

        }

        public IEnumerable<Auction> GetActiveAuctions()
        {
            return _getActiveAuctionsQueryFactory.Create().Execute();
        }

        public Auction GetAuction(Guid id)
        {
            id.ThrowIfNull("id");

            var getAuctionQuery = _getAuctionQueryFactory.Create();
            getAuctionQuery.Id = id;

            return getAuctionQuery.Execute();
        }

        public Auction StartAuction(string title, IEnumerable<Item> items)
        {
            return StartAuction(title, items, 0);
        }

        public Auction StartAuction(string title, IEnumerable<Item> items, decimal openingPrice)
        {
            // Construct auction before creating command so that Auction validations will fire
            var auction = new Auction(title, items);

            var startAuctionCommand = _startAuctionCommandFactory.Create();
            startAuctionCommand.Auction = auction;
            startAuctionCommand.Execute();

            return startAuctionCommand.Auction;
        }

        public Bid PlaceBid(Guid auctionId, Guid bidderId, decimal amount)
        {
            return GetAuction(auctionId).PlaceBid(bidderId, amount);
        }

        public void EndAuction(Guid id)
        {
            id.ThrowIfNull("id");

            GetAuction(id).EndAuction();
        }
    }
}