using AuctionBlock.Domain.Model;
using AuctionBlock.Infrastructure.Extensions;
using NHibernate;

namespace AuctionBlock.DataAccess.Commands
{
    public class StartAuctionCommand : 
        NHibernateCommand, IStartAuctionCommand
    {
        public Auction Auction { get; set; }

        public StartAuctionCommand(ISession session) 
            : base(session)
        {
        }

        public override void Execute()
        {
            this.Auction.ThrowIfNull("Auction");

            Session.Save(Auction);
        }
    }
}