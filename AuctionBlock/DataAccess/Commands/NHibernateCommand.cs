using NHibernate;

namespace AuctionBlock.DataAccess.Commands
{
    public abstract class NHibernateCommand
        : ICommand
    {
        protected ISession Session;

        protected NHibernateCommand(ISession session)
        {
            Session = session;
        }

        public abstract void Execute();
    }
}