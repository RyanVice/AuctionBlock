using System;
using System.Collections.Generic;
using AuctionBlock.Domain.Model;

namespace AuctionBlock.Domain.Services
{
    public interface IAuctionBlockService
    {
        IEnumerable<Auction> GetActiveAuctions();
        Auction GetAuction(Guid id);
        Auction StartAuction(Auction.Configuration configuration = null);
        void EndAuction(Guid id);
        Bid PlaceBid(Guid auctionId, Guid bidderId, decimal amount);
    }
}