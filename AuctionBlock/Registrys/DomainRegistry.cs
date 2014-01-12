using AuctionBlock.DataAccess.Commands;
using AuctionBlock.DataAccess.Queries;
using StructureMap.Configuration.DSL;

namespace AuctionBlock.Registrys
{
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