using AuctionBlock.Models;
using AutoMapper;
using StructureMap;

namespace AuctionBlock.Mapping
{
    public static class AutoMapperBootstrapper
    {
        private static readonly IConfiguration Configuration 
            = ObjectFactory.GetInstance<IConfiguration>();

        public static void Initialize()
        {
            Configuration.AddProfile<DtoProfile>();
        }
    }
}