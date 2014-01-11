using AuctionBlock.Domain.Model;

namespace AuctionBlock.DataAccess.Mappings
{
    public class ItemMap : DomainClassMap<Item>
    {
        public ItemMap()
        {
            Table("Items");
            Id(x => x.Id);
            Map(x => x.Description).Not.Nullable();
            References(x => x.Auction);
        }
    }
}