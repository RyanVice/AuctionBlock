using AuctionBlock.Domain.Model;

namespace AuctionBlock.DataAccess.Commands
{
    public interface IStartAuctionCommand : ICommand
    {
        Auction Auction { get; set; }
    }
}