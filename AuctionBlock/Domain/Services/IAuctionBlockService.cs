using System;
using System.Collections.Generic;
using AuctionBlock.Domain.Model;

namespace AuctionBlock.Domain.Services
{
    public interface IAuctionBlockService
    {
        IEnumerable<Auction> GetActiveAuctions();
        Auction GetAuction(Guid id);
        Auction StartAuction(string title, IEnumerable<Item> items);
        Auction StartAuction(string title, IEnumerable<Item> items, decimal openingPrice);
        void EndAuction(Guid id);
        Bid PlaceBid(Guid auctionId, Guid bidderId, decimal amount);
    }
}