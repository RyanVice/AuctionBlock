using System.Collections.Generic;

namespace AuctionBlock.Models.Response
{
    public class LotResponse
    {
        public IEnumerable<ItemResponse> Items { get; set; }
    }
}