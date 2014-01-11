using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AuctionBlock.Domain.Services;
using AuctionBlock.Models.Request;
using AuctionBlock.Models.Response;
using AutoMapper;

namespace AuctionBlock.Controllers
{
    public class AuctionBidsController : ApiController
    {
        private readonly IAuctionBlockService _auctionBlockService;
        private readonly IMappingEngine _mappingEngine;

        public AuctionBidsController(
            IAuctionBlockService auctionBlockService,
            IMappingEngine mappingEngine)
        {
            _auctionBlockService = auctionBlockService;
            _mappingEngine = mappingEngine;
        }

        public HttpResponseMessage Get(HttpRequestMessage request, Guid auctionId)
        {
            return request.CreateResponse(
                HttpStatusCode.OK,
                _auctionBlockService
                    .GetAuction(auctionId)
                    .Bids
                    .Select(bid => _mappingEngine.Map<BidResponse>(bid)));
        }

        public HttpResponseMessage Post(
            HttpRequestMessage request,
            Guid auctionId,
            BidPostRequest bidPostRequestRequest)
        {
            var bid = _mappingEngine.Map<BidResponse>(
                _auctionBlockService
                    .PlaceBid(
                        auctionId,
                        bidPostRequestRequest.BidderId,
                        bidPostRequestRequest.Amount));

            return request.CreateResponse(
                HttpStatusCode.Created,
                bid,
                "AuctionBids",
                new { auctionId, id = bid.Id });

        }
    }
}
