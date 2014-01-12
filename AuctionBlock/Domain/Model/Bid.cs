using System;

namespace AuctionBlock.Domain.Model
{
    public class Bid : Entity
    {
        public virtual decimal Amount { get; set; }
        public virtual Auction Auction { get; set; }
        public virtual Bidder Bidder { get; set; }
        public virtual DateTimeOffset PlacedOn { get; set; }

        public Bid()
        {
        }
    }
}