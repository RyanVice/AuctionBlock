using System;
using System.Collections.Generic;
using AuctionBlock.Domain.Model;

namespace AuctionBlock.DataAccess.Queries
{
    public interface IGetAuctionQuery
        : IQuery<Auction>
    {
        Guid Id { get; set; }
    }
}