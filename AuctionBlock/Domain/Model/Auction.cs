using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public virtual decimal ReservePrice { get; protected set; }
        public virtual decimal Increment { get; protected set; }
        public virtual decimal OpeningPrice { get; protected set; }
        public virtual IList<Item> Items { get; protected set; }
        public virtual DateTimeOffset StartedAt { get; protected set; }

        protected Auction()
        {
            Initialize();
        }

        public Auction(Configuration configuration)
        {

            configuration.ThrowIfNull("configuration");

            Initialize();

            configuration.Configure(this);
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

        public class Configuration
        {
            private readonly string _title;
            private readonly IEnumerable<Item> _items;
            public decimal ReservePrice { get; set; }
            public decimal Increment { get; set; }
            public decimal OpeningPrice { get; set; }

            public Configuration(string title, IEnumerable<Item> items)
            {
                title.ThrowIfNullEmptyOrWhiteSpace("title");
                items.ThrowIfNullOrEmpty("items");
                _title = title;
                _items = items;
            }

            public void Configure(Auction auction)
            {
                SetRequiredConfigurations(auction);
                TrySetOptionalConfigurations(auction);
            }

            private void SetRequiredConfigurations(Auction auction)
            {
                auction.Title = _title;
                foreach (var item in _items)
                {
                    item.Auction = auction;
                    auction.Items.Add(item);
                }
            }

            private void TrySetOptionalConfigurations(Auction auction)
            {
                auction.ReservePrice = TryGetValidConfiguration(() => ReservePrice);
                auction.Increment = TryGetValidConfiguration(() => Increment);
                auction.OpeningPrice = TryGetValidConfiguration(() => OpeningPrice);
            }

            private decimal TryGetValidConfiguration(Expression<Func<decimal>> expression)
            {
                decimal result = expression.Compile().Invoke();

                result.ThrowConfigurationExceptionIf(
                    x => x >= 0,
                    string.Format(
                        "Value of {0} must be greater than 0.",
                        expression.GetProperty()));

                return result;
            }
        }
    }
}