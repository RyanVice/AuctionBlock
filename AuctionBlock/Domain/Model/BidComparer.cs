using System.Collections.Generic;

namespace AuctionBlock.Domain.Model
{
    internal class BidComparer : IComparer<Bid>
    {
        public int Compare(Bid x, Bid y)
        {
            if (x.PlacedOn == y.PlacedOn)
                return 0;
            if (x.PlacedOn < y.PlacedOn)
                return -1;
            return 1;
        }
    }
}