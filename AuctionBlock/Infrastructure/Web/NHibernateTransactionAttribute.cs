using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NHibernate;
using NHibernate.Context;
using StructureMap;

namespace AuctionBlock.Infrastructure.Web
{
    public class NhSessionManagementAttribute : ActionFilterAttribute
    {
        private ISession _session;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _session = ObjectFactory.GetInstance<ISession>();
            CurrentSessionContext.Bind(_session);
            _session.BeginTransaction();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var transaction = _session.Transaction;
            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
            }
            _session = CurrentSessionContext.Unbind(
                ObjectFactory.GetInstance<ISessionFactory>());
            _session.Close();
        }
    }
}