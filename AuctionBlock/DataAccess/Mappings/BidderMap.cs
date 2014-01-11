using AuctionBlock.Domain.Model;

namespace AuctionBlock.DataAccess.Mappings
{
    public class BidderMap : DomainClassMap<Bidder>
    {
        public BidderMap()
        {
            Map(x => x.Name).Length(50).Not.Nullable();
        }
    }
}