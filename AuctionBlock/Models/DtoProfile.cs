using System;
using System.Collections.Generic;
using AuctionBlock.Domain.Model;
using AuctionBlock.Models.Request;
using AuctionBlock.Models.Response;
using AutoMapper;

namespace AuctionBlock.Models
{
    public class DtoProfile : Profile
    {
        protected override void Configure()
        {
            MapAuctions();

            Mapper.CreateMap<Bid, BidResponse>()
                  .ForMember(
                    dst => dst.BidderId, 
                    opt => opt.MapFrom(src => src.Bidder.Id))
                    .ForMember(
                        dst => dst.AuctionId, 
                        opt => opt.MapFrom(src => src.Auction.Id));
        }

        private void MapAuctions()
        {
            Mapper.CreateMap<ItemRequest, Item>();
            Mapper.CreateMap<Auction, AuctionResponse>();
            Mapper.CreateMap<Item, ItemResponse>();
            Mapper.CreateMap<Bid, BidResponse>()
                .ForMember(
                    dst => dst.PlacedOn, 
                    opt => opt.ResolveUsing(
                        dst => dst.PlacedOn.DateTime));
            Mapper.CreateMap<
                AuctionConfigurationRequest,
                Auction.Configuration>()
                  .ConstructUsing(GetAuctionConfiguration);
        }

        private Auction.Configuration GetAuctionConfiguration(AuctionConfigurationRequest configurationRequest)
        {
            return new Auction.Configuration(
                configurationRequest.Title,
                Mapper.Map<IEnumerable<Item>>(configurationRequest.Items));
        }
    }
}