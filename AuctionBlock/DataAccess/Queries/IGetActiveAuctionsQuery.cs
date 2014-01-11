using System.Collections.Generic;
using AuctionBlock.Domain.Model;

namespace AuctionBlock.DataAccess.Queries
{
    public interface IGetActiveAuctionsQuery 
        : IQuery<IEnumerable<Auction>> 
    {
    }
}
