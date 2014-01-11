using AuctionBlock.Domain.Model;
using FluentNHibernate.Mapping;

namespace AuctionBlock.DataAccess.Mappings
{
    public abstract class DomainClassMap<T> : ClassMap<T>
        where T : Entity
    {
        protected DomainClassMap()
        {
            Id(x => x.Id);
        }
    }
}