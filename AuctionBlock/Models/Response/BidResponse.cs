using System;

namespace AuctionBlock.Models.Response
{
    public class BidResponse
    {
        public decimal Amount { get; set; }
        public Guid AuctionId { get; set; }
        public Guid BidderId { get; set; }
        public DateTime PlacedOn { get; set; }
        public Guid Id { get; set; }
    }
}