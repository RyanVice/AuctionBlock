using AuctionBlock.DataAccess.Commands;
using AuctionBlock.DataAccess.Queries;
using AuctionBlock.Domain.Services;
using StructureMap.Configuration.DSL;

namespace AuctionBlock.Registrys
{
    public class DomainRegistry : Registry
    {
        public DomainRegistry()
        {
            For<IAuctionBlockService>()
                .Singleton().Use<AuctionBlockService>();
        }
    }

    public class DataAccessRegistry : Registry
    {
        public DataAccessRegistry()
        {
            For<IGetActiveAuctionsQuery>()
                .Use<GetActiveAuctionsQuery>();
            For<IGetAuctionQuery>()
                .Use<GetAuctionQuery>();
            For<IStartAuctionCommand>()
                .Use<StartAuctionCommand>();
        }
    }
}