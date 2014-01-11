using System;

namespace AuctionBlock.Domain.Model
{
    public class Bidder : Entity
    {
        public virtual string Name { get; set; }

        protected Bidder()
        {}

        public Bidder(string name)
        {
            Name = name;
        }

        public Bidder(Guid id)
        {
            Id = id;
        }
    }
}