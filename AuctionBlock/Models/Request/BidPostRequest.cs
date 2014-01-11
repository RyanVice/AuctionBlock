using System;

namespace AuctionBlock.Models.Request
{
    public class BidPostRequest
    {
        public decimal Amount { get; set; }
        public Guid BidderId { get; set; }
    }
}