using System;
using System.Collections.Generic;

namespace AuctionBlock.Models.Response
{
    public class AuctionResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<ItemResponse> Items { get; set; }
        public IEnumerable<BidResponse> Bids { get; set; } 
    }
}