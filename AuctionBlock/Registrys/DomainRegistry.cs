using AuctionBlock.Domain.Services;
using StructureMap.Configuration.DSL;

namespace AuctionBlock.Registrys
{
    public class DomainRegistry : Registry
    {
        public DomainRegistry()
        {
            For<IAuctionService>()
                .Use<AuctionService>();
        }
    }
}