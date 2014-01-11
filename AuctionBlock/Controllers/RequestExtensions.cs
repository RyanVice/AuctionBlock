using System;
using System.Net;
using System.Net.Http;

namespace AuctionBlock.Controllers
{
    public static class RequestExtensions
    {
        public static HttpResponseMessage CreateResponse(
            this HttpRequestMessage @this, 
            HttpStatusCode httpStatusCode, 
            object body, 
            string routeName,
            object routeValues)
        {
            HttpResponseMessage httpResponseMessage = @this.CreateResponse(httpStatusCode, body);

            string uriString = @this.GetUrlHelper().Link(routeName, routeValues);
            
            httpResponseMessage.Headers.Location = new Uri(uriString);

            return httpResponseMessage;
        }
    }
}