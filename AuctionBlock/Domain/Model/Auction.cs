using System;
using System.Collections.Generic;
using System.Linq;
using AuctionBlock.Infrastructure.Extensions;

namespace AuctionBlock.Domain.Model
{
    public class Auction : Entity
    {
        public virtual string Title { get; protected set; }
        private IList<Bid> _bids;
        public virtual IList<Bid> Bids { get { return _bids; } }
        public virtual Bid CurrentBid
        {
            get 
            { 
                return (_bids == null || !Bids.Any()) 
                    ? null : 
                    _bids.First(x => x.Amount == _bids.Max(b => b.Amount)); 
            }
        }
        public virtual AuctionStatus Status { get; protected set; }
        public virtual decimal OpeningPrice { get; protected set; }
        public virtual IList<Item> Items { get; protected set; }
        public virtual DateTimeOffset StartedAt { get; protected set; }

        protected Auction()
        {
            Initialize();
        }

        public Auction(string title, IEnumerable<Item> items)
            : this(title, items, 0)
        {
        }

        public Auction(string title, IEnumerable<Item> items, decimal openingPrice)
        {
            title.ThrowIfNullEmptyOrWhiteSpace("title");
            items.ThrowIfNullOrEmpty("items");
            openingPrice.ThrowConfigurationExceptionIf(
                d => d >= 0, "openingPrice must be greater than or equal to 0");

            Initialize();

            Configure(title, items, openingPrice);
        }

        private void Configure(string title, IEnumerable<Item> items, decimal openingPrice)
        {
            Title = title;
            foreach (var item in items)
            {
                item.Auction = this;
                Items.Add(item);
            }

            OpeningPrice = openingPrice;
        }

        private void Initialize()
        {
            _bids = new List<Bid>();
            Items = new List<Item>();
            Status = AuctionStatus.Started;
            StartedAt = DateTime.UtcNow;
        }

        public virtual Bid PlaceBid(Guid bidderId, decimal bidAmount)
        {
            if (CurrentBid != null && CurrentBid.Amount >= bidAmount)
                throw new ArgumentException(
                    string.Format(
                        "New bids must be higher than current maximum bid of {0}", 
                        CurrentBid.Amount));

            var bid = new Bid {Amount = bidAmount, Auction = this, Bidder = new Bidder(bidderId)};
            _bids.Add(bid);

            return bid;
        }

        public virtual void EndAuction()
        {
            Status = AuctionStatus.Completed;
        }
    }
}