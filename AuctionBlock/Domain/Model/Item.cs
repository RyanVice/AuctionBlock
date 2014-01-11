using AuctionBlock.Infrastructure.Extensions;

namespace AuctionBlock.Domain.Model
{
    public class Item : Entity
    {
        public virtual string Description { get; set; }
        public virtual Auction Auction { get; set; }

        protected Item()
        {}

        public Item(string description) 
        {
            description.ThrowIfNullEmptyOrWhiteSpace("description");
            Description = description;
        }
    }
}