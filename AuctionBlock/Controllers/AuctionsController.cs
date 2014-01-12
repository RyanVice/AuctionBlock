using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AuctionBlock.DataAccess.Commands;
using AuctionBlock.DataAccess.Queries;
using AuctionBlock.Domain.Model;
using AuctionBlock.Infrastructure.Factories;
using AuctionBlock.Models.Request;
using AuctionBlock.Models.Response;
using AutoMapper;

namespace AuctionBlock.Controllers
{
    public class AuctionsController : ApiController
    {
        private readonly IFactory<IStartAuctionCommand> _startAuctionCommandFactory;
        private readonly IFactory<IGetAuctionQuery> _getAuctionQueryFactory;
        private readonly IFactory<IGetActiveAuctionsQuery> _getActiveAuctionsQueryFactory;
        private readonly IMappingEngine _mappingEngine;

        public AuctionsController(
            IFactory<IStartAuctionCommand> startAuctionCommandFactory,
            IFactory<IGetAuctionQuery> getAuctionQueryFactory,
            IFactory<IGetActiveAuctionsQuery> getActiveAuctionsQueryFactory,
            IMappingEngine mappingEngine)
        {
            _startAuctionCommandFactory = startAuctionCommandFactory;
            _getAuctionQueryFactory = getAuctionQueryFactory;
            _getActiveAuctionsQueryFactory = getActiveAuctionsQueryFactory;
            _mappingEngine = mappingEngine;
        }

        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return request.CreateResponse(
                HttpStatusCode.OK,
                _mappingEngine.Map<IEnumerable<Auction>, AuctionResponse[]>(
                    _getActiveAuctionsQueryFactory.Create().Execute()));
        }

        public HttpResponseMessage Get(HttpRequestMessage request, Guid id)
        {
            var getAuctionQuery = _getAuctionQueryFactory.Create();
            getAuctionQuery.Id = id;

            return request.CreateResponse(
                HttpStatusCode.OK,
                getAuctionQuery.Execute());
        }

        public HttpResponseMessage Post(
            HttpRequestMessage request, 
            AuctionConfigurationRequest configurationRequest)
        {
            // Construct Auction before creating command so that Auction validations 
            // will fire before we build the command
            var auction = new Auction(
                        configurationRequest.Title, 
                        _mappingEngine.Map<IEnumerable<Item>>(
                            configurationRequest.Items),
                        configurationRequest.OpeningPrice);

            var startAuctionCommand = _startAuctionCommandFactory.Create();
            startAuctionCommand.Auction = auction;
            startAuctionCommand.Execute();

            var auctionResponse 
                = _mappingEngine.Map<AuctionResponse>(auction);

            return request.CreateResponse(
                HttpStatusCode.Created,
                auctionResponse, 
                "DefaultApi", 
                new { id = auction.Id });
        }

        public HttpResponseMessage End(Guid id)
        {
            var getAuctionQuery = _getAuctionQueryFactory.Create();
            getAuctionQuery.Id = id;

            getAuctionQuery.Execute().EndAuction();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}