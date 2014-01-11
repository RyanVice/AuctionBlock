using System.Collections.Generic;
using System.Linq;
using AuctionBlock.Domain.Model;
using NHibernate;
using NHibernate.Linq;

namespace AuctionBlock.DataAccess.Queries
{
    public class GetActiveAuctionsQuery : 
        NHibernateQuery<IEnumerable<Auction>>, 
        IGetActiveAuctionsQuery
    {
        public GetActiveAuctionsQuery(ISession session) 
            : base(session)
        {
        }

        public override IEnumerable<Auction> Execute()
        {
            return Session
                .Query<Auction>()
                .Where(auction => auction.Status == AuctionStatus.Started)
                .ToList();
        }
    }
}