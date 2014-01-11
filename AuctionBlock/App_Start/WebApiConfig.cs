using System.Web.Http;
using AuctionBlock.Infrastructure.Web;
using Newtonsoft.Json.Serialization;

namespace AuctionBlock
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ConfigureRoutes(config);

            ConfigureFilters(config);

            ConfigureFormatters(config);
        }

        private static void ConfigureFormatters(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver
                = new CamelCasePropertyNamesContractResolver();
        }

        private static void ConfigureFilters(HttpConfiguration config)
        {
            config.Filters.Add(new NhSessionManagementAttribute());
            config.Filters.Add(new ArguementExceptionFilterAttribute());
        }

        private static void ConfigureRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "AuctionBids",
                routeTemplate: "api/auctions/{auctionId}/bids/{id}",
                defaults: new {controller = "AuctionBids", id = RouteParameter.Optional}
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new {id = RouteParameter.Optional, action = RouteParameter.Optional}
                );
        }
    }
}
