using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AuctionBlock.Domain.Model;
using AuctionBlock.Domain.Services;
using AuctionBlock.Models.Request;
using AuctionBlock.Models.Response;
using AutoMapper;

namespace AuctionBlock.Controllers
{
    public class AuctionBidsController : ApiController
    {
        private readonly IAuctionService _auctionService;
        private readonly IMappingEngine _mappingEngine;

        public AuctionBidsController(
            IAuctionService auctionService,
            IMappingEngine mappingEngine)
        {
            _auctionService = auctionService;
            _mappingEngine = mappingEngine;
        }

        public HttpResponseMessage Get(HttpRequestMessage request, Guid auctionId)
        {
            return request.CreateResponse(
                HttpStatusCode.OK,
                GetAuction(auctionId)
                    .Bids
                    .Select(bid => _mappingEngine.Map<BidResponse>(bid)));
        }

        public HttpResponseMessage Post(
            HttpRequestMessage request,
            Guid auctionId,
            BidPostRequest bidPostRequestRequest)
        {
            var bid = _mappingEngine.Map<BidResponse>(
                GetAuction(auctionId).PlaceBid(
                        bidPostRequestRequest.BidderId,
                        bidPostRequestRequest.Amount));

            return request.CreateResponse(
                HttpStatusCode.Created,
                bid,
                "AuctionBids",
                new { auctionId, id = bid.Id });

        }

        private Auction GetAuction(Guid auctionId)
        {
            return _auctionService.GetAuction(auctionId);
        }
    }
}
