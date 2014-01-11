using System;
using System.Collections.Generic;
using AuctionBlock.Domain.Model;
using AuctionBlock.Infrastructure.Extensions;
using NHibernate;

namespace AuctionBlock.DataAccess.Queries
{
    public class GetAuctionQuery : 
        NHibernateQuery<Auction>, 
        IGetAuctionQuery
    {
        public GetAuctionQuery(ISession session) : base(session)
        {
        }

        public override Auction Execute()
        {
            Id.ThrowIfNull("Id");

            var auction = Session.Get<Auction>(Id);

            auction.ThrowIfNull(new KeyNotFoundException(Id.ToString()));

            return auction;
        }

        public Guid Id { get; set; }
    }
}