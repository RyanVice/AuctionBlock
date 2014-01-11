using NHibernate;

namespace AuctionBlock.DataAccess.Queries
{
    public abstract class NHibernateQuery<TResult> 
        : IQuery<TResult>
    {
        protected ISession Session;

        protected NHibernateQuery(ISession session)
        {
            Session = session;
        }

        public abstract TResult Execute();
    }
}