using AuctionBlock.Infrastructure.Factories;
using StructureMap.Configuration.DSL;

namespace AuctionBlock.Registrys
{
    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For(typeof(IFactory<>))
                .Use(typeof(StructureMapFactory<>));
        }
    }
}