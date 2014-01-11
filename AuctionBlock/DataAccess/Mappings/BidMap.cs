using AuctionBlock.Domain.Model;

namespace AuctionBlock.DataAccess.Mappings
{
    public class BidMap : DomainClassMap<Bid>
    {
        public BidMap()
        {
            Table("Bids");
            Map(x => x.Amount).CustomType<decimal>().Not.Nullable();
            Map(x => x.PlacedOn).Not.Nullable();
            References(x => x.Bidder);
            References(x => x.Auction);
        }
    }
}