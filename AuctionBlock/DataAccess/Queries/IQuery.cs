namespace AuctionBlock.DataAccess.Queries
{
    public interface IQuery<TResult>
    {
        TResult Execute();      
    }
}