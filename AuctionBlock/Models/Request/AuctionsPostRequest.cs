using System.Collections.Generic;

namespace AuctionBlock.Models.Request
{
    public class AuctionConfigurationRequest
    {
        public string Title { get; set; }
        public IEnumerable<ItemRequest> Items { get; set; }
        public decimal OpeningPrice { get; set; }
    }
}