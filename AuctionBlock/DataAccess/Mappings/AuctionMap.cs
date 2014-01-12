using AuctionBlock.Domain.Model;
using FluentNHibernate.Mapping;

namespace AuctionBlock.DataAccess.Mappings
{
    public class AuctionMap : DomainClassMap<Auction>
    {
        public AuctionMap()
        {
            Table("Auctions");
            Map(x => x.OpeningPrice).CustomType<decimal>().Nullable();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Status).CustomType<int>().Nullable();
            Map(x => x.StartedAt).Not.Nullable();
            HasMany(x => x.Bids)
                .Cascade.All().Inverse()
                .Access.CamelCaseField(Prefix.Underscore)
                .OrderBy("PlacedOn DESC");
            HasMany(x => x.Items).Cascade.All().Inverse();
        }
    }
}