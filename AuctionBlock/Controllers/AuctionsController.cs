using System;
using System.Collections.Generic;
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
    public class AuctionsController : ApiController
    {
        private readonly IAuctionService _auctionService;
        private readonly IMappingEngine _mappingEngine;

        public AuctionsController(
            IAuctionService auctionService,
            IMappingEngine mappingEngine)
        {
            _auctionService = auctionService;
            _mappingEngine = mappingEngine;
        }

        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return request.CreateResponse(
                HttpStatusCode.OK,
                _mappingEngine.Map<IEnumerable<Auction>, AuctionResponse[]>(
                    _auctionService.GetActiveAuctions()));
        }

        public HttpResponseMessage Get(HttpRequestMessage request, Guid id)
        {
            return request.CreateResponse(
                HttpStatusCode.OK,
                _auctionService.GetAuction(id));
        }

        public HttpResponseMessage Post(HttpRequestMessage request, AuctionConfigurationRequest configurationRequest)
        {
            var auction
                = _mappingEngine.Map<AuctionResponse>(
                    _auctionService.StartAuction(
                        configurationRequest.Title,
                        _mappingEngine.Map<IEnumerable<Item>>(configurationRequest.Items),
                        configurationRequest.OpeningPrice));

            return request.CreateResponse(
                HttpStatusCode.Created,
                auction,
                "DefaultApi",
                new { id = auction.Id });
        }

        public HttpResponseMessage End(Guid id)
        {
            _auctionService.EndAuction(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}