using System;
using System.Collections.Generic;
using System.Linq;
using AuctionBlock.Domain.Model;
using AuctionBlock.Infrastructure.Extensions;
using NHibernate;
using NHibernate.Linq;

namespace AuctionBlock.Domain.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly ISession _session;

        public AuctionService(ISession session)
        {
            _session = session;
        }

        public IEnumerable<Auction> GetActiveAuctions()
        {
            return _session
                .Query<Auction>()
                .Where(auction => auction.Status == AuctionStatus.Started)
                .ToList(); ;
        }

        public Auction GetAuction(Guid id)
        {
            id.ThrowIfNullOrEmpty(
                "You must provided a non-null, non-empty ID");

            var auction = _session.Get<Auction>(id);

            auction.ThrowIfNull(new KeyNotFoundException(id.ToString()));

            return auction;
        }

        public Auction StartAuction(string title, IEnumerable<Item> items)
        {
            return StartAuction(title, items, 0);
        }

        public Auction StartAuction(string title, IEnumerable<Item> items, decimal openingPrice)
        {
            var auction = new Auction(title, items, openingPrice);

            _session.Save(auction);

            return auction;
        }

        public Bid PlaceBid(Guid auctionId, Guid bidderId, decimal amount)
        {
            var auction = GetAuction(auctionId);

            var bid = auction.PlaceBid(bidderId, amount);

            _session.Save(auction);

            return bid;
        }

        public void EndAuction(Guid id)
        {
            GetAuction(id).EndAuction();
        }
    }
}