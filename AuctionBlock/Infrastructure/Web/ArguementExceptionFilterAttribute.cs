using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Filters;

namespace AuctionBlock.Infrastructure.Web
{
    public class ArguementExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ArgumentException
                || context.Exception is ArgumentNullException
                || context.Exception is ArgumentOutOfRangeException)
            {
                context.Response 
                    = context.Request.CreateResponse(
                        HttpStatusCode.BadRequest,
                        new HttpError(context.Exception.Message));
            }
        }
    }
}